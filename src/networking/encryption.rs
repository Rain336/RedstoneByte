use aes::Aes128;
use bytes::{Bytes, BytesMut};
use cfb8::stream_cipher::{InvalidKeyNonceLength, NewStreamCipher, StreamCipher};
use cfb8::Cfb8;
use std::io;

type AesCfb8 = Cfb8<Aes128>;

pub struct EncryptionInfo(AesCfb8);

impl EncryptionInfo {
    pub fn new(key: &[u8]) -> Result<Self, InvalidKeyNonceLength> {
        Ok(EncryptionInfo(AesCfb8::new_var(key, key)?))
    }

    pub fn decrypt_packet(&mut self, packet: Bytes) -> Bytes {
        let mut result = BytesMut::from(&packet[..]);
        self.0.decrypt(&mut result);
        result.freeze()
    }

    pub fn encrypt_packet(&mut self, packet: Bytes) -> Bytes {
        let mut result = BytesMut::from(&packet[..]);
        self.0.encrypt(&mut result);
        result.freeze()
    }
}
