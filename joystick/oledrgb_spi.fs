\ ---------------------------------------------
\ -- Module: OLEDRGB_SPI in ANS-FORTH ---------
\ -- Author: Thorsten Knoll -------------------
\ -- Date:   March 2017 -----------------------
\ -- Written for J1A CPU ----------------------
\ ---------------------------------------------

hex

1  CONSTANT CS-PIN
2  CONSTANT MOSI-PIN
8  CONSTANT SCK-PIN
10 CONSTANT DC-PIN
20 CONSTANT RES-PIN
40 CONSTANT VCCEN-PIN
80 CONSTANT PMODEN-PIN

$0001 CONSTANT PMOD-DATA
$0002 CONSTANT PMOD-DIR

VARIABLE PMOD-STATE

: US
  0 DO 6 0 DO LOOP LOOP ;

: PMOD-PINS!
  PMOD-DATA io! ;

: PMOD-DIR!
  FF PMOD-DIR io! ;

: PMOD-ZERO!
  0 PMOD-STATE !
  PMOD-STATE @ PMOD-PINS! ;

: LOW ( pin --- )
  FF XOR 
  PMOD-STATE @ AND
  DUP PMOD-STATE !
  PMOD-PINS! ;

: HIGH ( pin --- )
  PMOD-STATE @ OR
  DUP PMOD-STATE !
  PMOD-PINS! ;

: SPI-SEND ( n --- )
  CS-PIN LOW
  DC-PIN LOW
  0 7 DO
    SCK-PIN HIGH
    DUP
    I RSHIFT
    1 AND
    IF 
      MOSI-PIN HIGH
    ELSE 
      MOSI-PIN LOW
    THEN
    F US
    SCK-PIN LOW
    F US
  -1 +LOOP
  SCK-PIN HIGH
  DROP
  MOSI-PIN LOW
  DC-PIN HIGH
  CS-PIN HIGH ;

