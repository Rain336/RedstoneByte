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
- [X] Custom Native Cipher based on OPenSSL

### Planed
- [ ] Plugin Api

### Bugs
- [ ] TheProxy stops passing through Packets after a while.
- [ ] Memoryleak in some ByteBuffer. Maybe fixed?