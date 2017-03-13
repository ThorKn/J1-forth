\ ---------------------------------------------
\ -- Module: HDR1-SPI in ANS-FORTH ------------
\ -- Author: Thorsten Knoll -------------------
\ -- Date:   March 2017 -----------------------
\ -- Written for J1A CPU ----------------------
\ ---------------------------------------------

hex

8 CONSTANT HDR1-CS-PIN
4 CONSTANT HDR1-MOSI-PIN
2 CONSTANT HDR1-MISO-PIN
1 CONSTANT HDR1-SCK-PIN

$0010 CONSTANT HDR1-DATA
$0020 CONSTANT HDR1-DIR

VARIABLE HDR1-STATE
VARIABLE HDR1-RECV 4 ALLOT


: HDR1-PINS!
  HDR1-DATA io! ;

: HDR1-DIR!
  FD HDR1-DIR io! ;

: HDR1-ZERO!
  0 HDR1-STATE !
  HDR1-STATE @ HDR1-PINS! ;


: HDR1-LOW ( pin --- )
  FF XOR 
  HDR1-STATE @ AND
  DUP HDR1-STATE !
  HDR1-PINS! ;

: HDR1-HIGH ( pin --- )
  HDR1-STATE @ OR
  DUP HDR1-STATE !
  HDR1-PINS! ;

: HDR1-SPI-SEND ( n --- )
  0 HDR1-RECV !
  0 HDR1-RECV 1 + C!
  0 HDR1-RECV 2 + C!
  0 HDR1-RECV 3 + C!
  0 HDR1-RECV 4 + C!

  5 0 DO
    HDR1-CS-PIN HDR1-LOW
    0 7 DO

      \ --- spi-mode 0 falling edge
      HDR1-SCK-PIN HDR1-LOW

      \ --- send bits
      DUP
      I RSHIFT
      1 AND
      IF 
        HDR1-MOSI-PIN HDR1-HIGH
      ELSE 
        HDR1-MOSI-PIN HDR1-LOW
      THEN

      \ --- spi-mode 0 rising edge
      1 MS
      HDR1-SCK-PIN HDR1-HIGH
      1 MS

      \ --- recv bits
      HDR1-DATA io@
      HDR1-MISO-PIN AND
      IF
        1 I LSHIFT
        HDR1-RECV J + C@
        OR
        HDR1-RECV J + C!
      THEN

      1 MS

    -1 +LOOP
  LOOP
  HDR1-SCK-PIN HDR1-LOW
  DROP
  HDR1-MOSI-PIN HDR1-LOW
  HDR1-CS-PIN HDR1-HIGH ;

: HDR1-RECV?
  5 0 DO 
    HDR1-RECV I + C@
    CR .
  LOOP
  CR
  ;




