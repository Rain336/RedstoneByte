use bytes::{Bytes, BytesMut};
use libdeflater::{CompressionLvl, Compressor, Decompressor};
use std::io;
use std::sync::Mutex;

pub struct CompressionInfo {
    decompressor: Mutex<Decompressor>,
    compressor: Mutex<Compressor>,
    threshold: u32,
}

impl CompressionInfo {
    pub fn new(threshold: u32) -> Self {
        CompressionInfo {
            decompressor: Mutex::new(Decompressor::new()),
            compressor: Mutex::new(Compressor::new(CompressionLvl::fastest())),
            threshold: threshold,
        }
    }

    pub async fn decompress_packet(&mut self, mut packet: Bytes) -> Result<Bytes, io::Error> {
        let uncompressed = super::read_var_int(&mut packet) as usize;
        if uncompressed == 0 {
            Ok(packet)
        } else {
            let mut result = BytesMut::with_capacity(uncompressed);
            let mut decompressor = self.decompressor.lock().unwrap();
            match decompressor.deflate_decompress(&packet, &mut result) {
                Ok(_) => Ok(result.freeze()),
                Err(error) => Err(io::Error::new(
                    io::ErrorKind::InvalidData,
                    "The compressed data is invalid!",
                )),
            }
        }
    }

    pub async fn compress_async(&mut self, packet: Bytes) -> Result<Bytes, io::Error> {
        if packet.len() <= self.threshold as usize {
            Ok(packet)
        } else {
            let prefix = super::var_int_size(packet.len() as u32);
            let mut compressor = self.compressor.lock().unwrap();
            let compressed = compressor.deflate_compress_bound(packet.len());
            let mut result = BytesMut::with_capacity(prefix + compressed);
            super::write_var_int(&mut result, packet.len() as u32);
            match compressor.deflate_compress(&packet, &mut result) {
                Ok(_) => Ok(result.freeze()),
                Err(_) => unreachable!(),
            }
        }
    }

    pub fn set_threshold(&mut self, value: u32) {
        self.threshold = value
    }
}
