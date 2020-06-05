use bytes::{BufMut, Buf};

pub fn read_var_int(read: &mut impl Buf) -> u32 {
    let mut value = 0;
    for i in 0..5 {
        let b = read.get_u8() as u32;
        value |= (b & 0x7F) << (i * 7);

        if (b & 0x80) == 0 {
            break;
        }
    }

    value
}

pub fn read_var_long(read: &mut impl Buf) -> u64 {
    let mut value = 0;
    for i in 0..10 {
        let b = read.get_u8() as u64;
        value |= (b & 0x7F) << (i * 7);

        if (b & 0x80) == 0 {
            break;
        }
    }

    value
}

pub fn write_var_int(buffer: &mut impl BufMut, mut value: u32) {
    while value != 0 {
        let mut b = (value as u8) & 0x7F;
        value >>= 7;

        if value != 0 {
            b |= 0x80;
        }

        buffer.put_u8(b);
    }
}

pub fn write_var_long(buffer: &mut impl BufMut, mut value: u64) {
    while value != 0 {
        let mut b = (value as u8) & 0x7F;
        value >>= 7;

        if value != 0 {
            b |= 0x80;
        }

        buffer.put_u8(b);
    }
}

pub fn var_int_size(value: u32) -> usize {
    if (value & 0xFFFFFF80) == 0 {
        1
    } else if (value & 0xFFFFC000) == 0 {
        2
    } else if (value & 0xFFE00000) == 0 {
        3
    } else if (value & 0xF0000000) == 0 {
        4
    } else {
        5
    }
}

pub fn var_long_size(value: u64) -> usize {
    if (value & 0xFFFFFFFFFFFFFF80) == 0 {
        1
    } else if (value & 0xFFFFFFFFFFFFC000) == 0 {
        2
    } else if (value & 0xFFFFFFFFFFE00000) == 0 {
        3
    } else if (value & 0xFFFFFFFFF0000000) == 0 {
        4
    } else if (value & 0xFFFFFFF800000000) == 0 {
        5
    } else if (value & 0xFFFFFC0000000000) == 0 {
        6
    } else if (value & 0xFFFE000000000000) == 0 {
        7
    } else if (value & 0xFF00000000000000) == 0 {
        8
    } else if (value & 0x8000000000000000) == 0 {
        9
    } else {
        10
    }
}