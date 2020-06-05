use std::io;
use bytes::{BytesMut, Bytes, Buf};
use bytes::buf::ext::BufExt;
use tokio::prelude::*;

pub async fn frame_decoder(read: &mut (impl AsyncRead + Unpin)) -> Result<Bytes, std::io::Error> {
    let mut value = 0;
    for i in 0..3 {
        let b = read.read_u8().await? as usize;
        value |= (b & 0x7F) << (i * 7);

        if (b & 0x80) != 0 {
            continue
        }

        let mut buffer = BytesMut::with_capacity(value);
        read.read_exact(&mut buffer).await?;

        return Ok(buffer.freeze());
    }
    
    Err(io::Error::new(io::ErrorKind::InvalidData, "Packet length is longer than 3 bytes."))
}

pub async fn frame_encoder(buffer: BytesMut) -> impl Buf {
    let length = buffer.len() as u32;
    let mut prefix = BytesMut::with_capacity(super::var_int_size(length));
    super::write_var_int(&mut prefix, length);
    prefix.freeze().chain(buffer.freeze())
}