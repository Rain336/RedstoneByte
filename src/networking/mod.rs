mod compression;
mod encryption;
mod frame;
mod var_int;

use std::net::SocketAddr;
use tokio::prelude::*;
pub use var_int::*;

pub struct Connection {
    address: SocketAddr,
    encryption: Option<encryption::EncryptionInfo>,
    compression: Option<compression::CompressionInfo>,
}

impl Connection {
    pub fn new(address: SocketAddr) -> Self {
        Connection {
            address,
            encryption: None,
            compression: None,
        }
    }

    pub async fn run(mut self, mut read: impl AsyncRead + Unpin) -> Result<(), std::io::Error> {
        loop {
            let mut packet = frame::frame_decoder(&mut read).await?;

            if let Some(ref mut e) = self.encryption {
                packet = e.decrypt_packet(packet);
            }

            if let Some(ref mut c) = self.compression {
                packet = c.decompress_packet(packet)?;
            }
        }
    }

    fn set_compression_threshold(&mut self, value: u32) {
        match self.compression {
            Some(ref mut c) => c.set_threshold(value),
            None => self.compression = Some(compression::CompressionInfo::new(value)),
        }
    }

    fn set_encrpytion_key(&mut self, key: &[u8]) -> Result<(), cfb8::stream_cipher::InvalidKeyNonceLength> {
        self.encryption = Some(encryption::EncryptionInfo::new(key)?);
        Ok(())
    }
}
