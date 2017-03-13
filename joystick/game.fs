\ ---------------------------------------------
\ -- Module: GAME for J1a in ANS-FORTH --------
\ ---------- with OLEDRGB and PMODJSTK --------
\ ---------- via SPI on PMOD and HDR1 ---------
\ -- Author: Thorsten Knoll -------------------
\ -- Date:   March 2017 -----------------------
\ -- Written for J1A CPU ----------------------
\ ---------------------------------------------

#include oledrgb.fs
#include hdr1.fs

: START-GAME
  OLED-INIT
  100 MS
  CLEAR
  HDR1-INIT
  JSTK-LEDS-ON
  BEGIN
    3 MS
    JSTK-LEDS-ON
    CLEAR
    HDR1-RECV @ 4 RSHIFT
    0 0 0 LINE
    HDR1-RECV 2 + @ 4 RSHIFT
    30 0 30 LINE
    HDR1-RECV 4 + @ 4 AND 4 =
  UNTIL
  CLEAR
  JSTK-LEDS-OFF
  ;

