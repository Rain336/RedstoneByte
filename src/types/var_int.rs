use serde::ser::{Serialize, Serializer, SerializeSeq};

pub struct VarInt {
    pub bytes: [u8; 5],
    pub size: u8 
}

impl From<i32> for VarInt {
    fn from(mut v: i32) -> Self { 
        let mut s = Self { bytes: [0; 5], size: 0 };

        loop {
            let mut tmp = (v & 0x7F) as u8;     
            v = (v as u32 >> 7) as i32;

            if v != 0 {
                tmp |= 0x80;
            } 

            s.bytes[s.size as usize] = tmp;
            s.size += 1;

            if v == 0 {
                break;
            } 
        }

        s
    }
}

impl From<VarInt> for i32 {
    fn from(v: VarInt) -> Self { 
        let mut r = 0;

        for i in 0..(v.size as usize) {
            r |= ((v.bytes[i] & 0x7f) as i32) << (7 * i);
        }

        r
    }
}

impl Serialize for VarInt {
    fn serialize<S>(&self, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: Serializer,
    {
        let mut state = serializer.serialize_seq(None)?;

        for i in 0..(self.size as usize) {
            state.serialize_element(&self.bytes[i])?;
        }

        state.end()
    }
}

#[test] 
fn conversion_varint() {
    assert_eq!(VarInt::from(0).bytes, [0x00u8, 0x00, 0x00, 0x00, 0x00]);
    assert_eq!(VarInt::from(1).bytes, [0x01u8, 0x00, 0x00, 0x00, 0x00]);
    assert_eq!(VarInt::from(2).bytes, [0x02u8, 0x00, 0x00, 0x00, 0x00]);
    assert_eq!(VarInt::from(127).bytes, [0x7fu8, 0x00, 0x00, 0x00, 0x00]);
    assert_eq!(VarInt::from(128).bytes, [0x80u8, 0x01, 0x00, 0x00, 0x00]);
    assert_eq!(VarInt::from(255).bytes, [0xffu8, 0x01, 0x00, 0x00, 0x00]);
    assert_eq!(VarInt::from(2147483647).bytes, [0xffu8, 0xff, 0xff, 0xff, 0x07]);
    assert_eq!(VarInt::from(-1).bytes, [0xffu8, 0xff, 0xff, 0xff, 0x0f]);
    assert_eq!(VarInt::from(-2147483648).bytes, [0x80u8, 0x80, 0x80, 0x80, 0x08]);
    
    assert_eq!(0, VarInt { bytes: [0x00u8, 0x00, 0x00, 0x00, 0x00], size: 1 }.into());
    assert_eq!(1, VarInt { bytes: [0x01u8, 0x00, 0x00, 0x00, 0x00], size: 1 }.into());
    assert_eq!(2, VarInt { bytes: [0x02u8, 0x00, 0x00, 0x00, 0x00], size: 1 }.into());
    assert_eq!(127, VarInt { bytes: [0x7fu8, 0x00, 0x00, 0x00, 0x00], size: 1 }.into());
    assert_eq!(128, VarInt { bytes: [0x80u8, 0x01, 0x00, 0x00, 0x00], size: 2 }.into());
    assert_eq!(255, VarInt { bytes: [0xffu8, 0x01, 0x00, 0x00, 0x00], size: 2 }.into());
    assert_eq!(2147483647, VarInt { bytes: [0xffu8, 0xff, 0xff, 0xff, 0x07], size: 5 }.into());
    assert_eq!(-1, VarInt { bytes: [0xffu8, 0xff, 0xff, 0xff, 0x0f], size: 5 }.into());
    assert_eq!(-2147483648, VarInt { bytes: [0x80u8, 0x80, 0x80, 0x80, 0x08], size: 5 }.into());
}

pub const fn vi(v: i32) -> VarInt {
    let mut s = VarInt { bytes: [0; 5], size: 0 };
    let mut v = v;

    // iter 1
    let mut tmp = (v & 0x7F) as u8;
    let c = v != 0;
    v = (v as u32 >> 7) as i32;

    tmp |= 0x80 * (v != 0) as u8;

    s.bytes[s.size as usize] = tmp;
    s.size += c as u8;

    // iter 2
    let mut tmp = (v & 0x7F) as u8;
    let c = v != 0;
    v = (v as u32 >> 7) as i32;

    tmp |= 0x80 * (v != 0) as u8;

    s.bytes[s.size as usize] = tmp;
    s.size += c as u8;

    // iter 3
    let mut tmp = (v & 0x7F) as u8;
    let c = v != 0;
    v = (v as u32 >> 7) as i32;

    tmp |= 0x80 * (v != 0) as u8;

    s.bytes[s.size as usize] = tmp;
    s.size += c as u8;

    // iter 4
    let mut tmp = (v & 0x7F) as u8;
    let c = v != 0;
    v = (v as u32 >> 7) as i32;

    tmp |= 0x80 * (v != 0) as u8;

    s.bytes[s.size as usize] = tmp;
    s.size += c as u8;

    // iter 5
    let mut tmp = (v & 0x7F) as u8;
    let c = v != 0;
    v = (v as u32 >> 7) as i32;

    tmp |= 0x80 * (v != 0) as u8;

    s.bytes[s.size as usize] = tmp;
    s.size += c as u8;

    s
}