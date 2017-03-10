This is the start of a library and maybe a little game for the J1 CPU and the Digilent Pmod OLEDRGB_96x64. The Pmod is driven by the SPI-Protocol.
So far it works with The J1A CPU from James Bowmans swapforth-implementation on an Lattice IceStick. The OLEDRGB-Pmod is attached to the IceStick via the Pmod-Connector with the standard pinlayout.

Get the OLEDRGB_96x64 to work:

1. Boot up the J1A on an IceStick
2. Get a connection to the shell.py on your PC
3. >#include oledrgb.fs
4. >oled-init
5. >endless

Notes:

- This project is purely for fun.
- It's work-in-progress and not nearly finished.
- The SPI Implementation is my first try on this protocol and is surely crappy. But hey... It works somehow!

Youtube video with the demo:
https://youtu.be/xrxGqdlCc10



