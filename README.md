# RedstoneByte - A Proxy for Minecraft Servers in C#
This is my attempt at writing something similar to BungeeCord.
I try to optimise it to hopefully be quicker than BungeeCord.

## Contributing
Feel free to make a pull request or write an issue when you have an idea for a feature.
The code style is the default ReSharpener code style.

## Features
### Compited
- [X] Client Pipeline
- [X] Version-independent Packet System
- [X] Text Api
- [X] NBT Api
- [X] Entity Id Patching

### Planed
- [ ] Replace BouncyCastle AES with .NET Core 2.0 AES
- [ ] Plugin Api

### Bugs
- [ ] Aes encryption doesn't work
- [ ] Packets break while passing through