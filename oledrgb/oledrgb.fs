\ ---------------------------------------------
\ -- Module: OLEDRGB in ANS-FORTH -------------
\ -- Author: Thorsten Knoll -------------------
\ -- Date:   March 2017 -----------------------
\ -- Written for J1A CPU ----------------------
\ ---------------------------------------------

#include oledrgb_spi.fs

: OLED-INIT

  \ --- Pmod direction to out
  FF $0002 io!

  \ --- Pin-state to zero
  0 PIN-STATE !
  PIN-STATE @ PINS!

  \ --- ChipSelect high
  CS-PIN HIGH  

  \ --- DC low
  DC-PIN LOW

  \ --- Reset high
  RES-PIN HIGH

  \ --- VccEn low
  VCCEN-PIN LOW

  \ --- PomEn high
  PMODEN-PIN HIGH
  20 MS

  \ --- Do reset
  RES-PIN LOW
  2 MS
  RES-PIN HIGH
  2 MS

  SCK-PIN HIGH
  1 MS

  \ --- unlock command
  FD SPI-SEND
  12 SPI-SEND

  \ --- display off command
  AE SPI-SEND

  \ --- remap and display format
  A0 SPI-SEND
  72 SPI-SEND

  \ --- startline to topline
  A1 SPI-SEND
  00 SPI-SEND
 
  \ --- no vertical offset
  A2 SPI-SEND
  00 SPI-SEND

  \ --- no color inversion
  A4 SPI-SEND

  \ --- multiplex ratio: all common pins enabled
  A8 SPI-SEND
  3F SPI-SEND

  \ --- external Vcc enabled
  AD SPI-SEND
  8E SPI-SEND

  \ --- disable power-saving mode
  B0 SPI-SEND
  0B SPI-SEND

  \ --- set the phase length
  B1 SPI-SEND
  31 SPI-SEND

  \ --- set clock divide ratio and osci frequency
  B3 SPI-SEND
  F0 SPI-SEND

  \ --- pre-charge speed of color A
  8A SPI-SEND
  64 SPI-SEND

  \ --- pre-charge speed of color B
  8B SPI-SEND
  78 SPI-SEND

  \ --- pre-charge speed of color C
  8C SPI-SEND
  64 SPI-SEND

  \ --- pre-charge voltage to 45% of Vcc
  BB SPI-SEND
  3A SPI-SEND

  \ --- VCOMH Deselct level
  BE SPI-SEND
  3E SPI-SEND

  \ --- Master current attentuation factor
  87 SPI-SEND
  06 SPI-SEND

  \ --- Contrast for color A
  81 SPI-SEND
  91 SPI-SEND

  \ --- Contrast for color B
  82 SPI-SEND
  50 SPI-SEND

  \ --- Contrast for color C
  83 SPI-SEND
  7D SPI-SEND

  \ --- disable scrolling
  25 SPI-SEND

  \ --- clear screen and window dimensions to clear
  25 SPI-SEND
  00 SPI-SEND
  00 SPI-SEND
  5F SPI-SEND
  3F SPI-SEND

  \ --- VCCEN high + 25ms wait
  VCCEN-PIN HIGH
  25 MS

  \ --- display on
  AF SPI-SEND
  100 MS ;

: OLED-OFF
  AE SPI-SEND
  VCCEN-PIN LOW
  400 MS ;

: CLEAR
  3F 5F 0 0 25
  5 0 DO SPI-SEND LOOP ;

: LINE ( y2, x2, y1, x1 --- )
  21 SPI-SEND
  4 0 DO SPI-SEND LOOP
  35 35 35
  3 0 DO SPI-SEND LOOP ;

: LINE-DMOVE
  5F 0 DO
    CLEAR
    3F I 0 5F I - LINE
    100 US
  LOOP
  0 5F DO
    CLEAR
    3F I 0 5F I - LINE
    100 US
  -1 +LOOP
  ;

: LINE-ROTATE
  5F 0 DO
    CLEAR
    3F I 0 5F I - LINE
    100 US
  LOOP
  3F 0 DO
    CLEAR
    I 0 3F I - 5F LINE
    100 US
  LOOP
  ;

: LINE-HMOVE
  5F 0 DO
    CLEAR
    3F I 0 I LINE
    100 US
  LOOP
  0 5F DO
    CLEAR
    3F I 0 I LINE
    100 US
  -1 +LOOP
  ;

: LINE-VMOVE
  3F 0 DO
    CLEAR
    I 5F I 0 LINE
    100 US
  LOOP
  0 3F DO
    CLEAR
    I 5F I 0 LINE
    100 US
  -1 +LOOP
  ;

: ENDLESS
  BEGIN
    3 0 DO
      LINE-ROTATE
    LOOP
    3 0 DO
      LINE-HMOVE
    LOOP
    3 0 DO
      LINE-VMOVE
    LOOP
  AGAIN
  ;





