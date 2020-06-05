use std::path::Path;
use serde::{Serialize, Deserialize};
use std::fs;

#[derive(Serialize, Deserialize, Debug)]
pub struct Configuration {
    pub address: String
}

impl Default for Configuration {
    fn default() -> Self {
        Configuration {
            address: "127.0.0.1:25565".to_string()
        }
    }
}

lazy_static! {
    pub static ref CONFIG: Configuration = {
        let path = Path::new("RedstoneByte.toml");
        if path.exists() {
            toml::from_str(&fs::read_to_string(path).unwrap()).unwrap()
        } else {
            let result = Configuration::default();
            fs::write(path, toml::to_string_pretty(&result).unwrap()).unwrap();
            result
        }
    };
}