\ ---------------------------------------------
\ -- Module: HDR1-PMOD-Joystick in ANS-FORTH --
\ -- Author: Thorsten Knoll -------------------
\ -- Date:   March 2017 -----------------------
\ -- Written for J1A CPU ----------------------
\ ---------------------------------------------

#include hdr1_spi.fs

: HDR1-INIT
  HDR1-DIR!
  HDR1-ZERO!
  HDR1-CS-PIN HDR1-HIGH
  HDR1-SCK-PIN HDR1-HIGH
  80 HDR1-SPI-SEND ;

: JSTK-LEDS-ON
  83 HDR1-SPI-SEND ;

: JSTK-LEDS-OFF
  80 HDR1-SPI-SEND ;

