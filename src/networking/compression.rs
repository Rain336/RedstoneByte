use bytes::{Bytes, BytesMut, BufMut, Buf};
use bytes::buf::ext::BufExt;
use libdeflater::{CompressionLvl, Compressor, Decompressor};
use std::io;

pub struct CompressionInfo {
    threshold: u32,
}

impl CompressionInfo {
    pub fn new(threshold: u32) -> Self {
        CompressionInfo {
            threshold: threshold,
        }
    }

    pub fn decompress_packet(&mut self, mut packet: Bytes) -> Result<Bytes, io::Error> {
        let uncompressed = super::read_var_int(&mut packet) as usize;
        if uncompressed == 0 {
            Ok(packet)
        } else {
            let mut result = BytesMut::with_capacity(uncompressed);
            let mut decompressor = Decompressor::new();
            match decompressor.deflate_decompress(&packet, &mut result) {
                Ok(_) => Ok(result.freeze()),
                Err(error) => Err(io::Error::new(
                    io::ErrorKind::InvalidData,
                    "The compressed data is invalid!",
                )),
            }
        }
    }

    pub fn compress_packet(&mut self, packet: Bytes) -> Result<impl Buf, io::Error> {
        if packet.len() <= self.threshold as usize {
            let mut prefix = BytesMut::with_capacity(1);
            prefix.put_u8(0);
            Ok(prefix.freeze().chain(packet))
        } else {
            let prefix = super::var_int_size(packet.len() as u32);
            let mut compressor = Compressor::new(CompressionLvl::fastest());
            let compressed = compressor.deflate_compress_bound(packet.len());
            let mut result = BytesMut::with_capacity(prefix + compressed);
            super::write_var_int(&mut result, packet.len() as u32);
            match compressor.deflate_compress(&packet, &mut result) {
                Ok(_) => Ok(result.freeze().chain(Bytes::new())),
                Err(_) => unreachable!(),
            }
        }
    }

    pub fn set_threshold(&mut self, value: u32) {
        self.threshold = value
    }
}
