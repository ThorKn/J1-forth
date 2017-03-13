\ ---------------------------------------------
\ -- Module: AES 128 in ANS-FORTH -------------
\ -- Author: Thorsten Knoll -------------------
\ -- Date:   Feb 2017 -------------------------
\ -- Written for J1Sc CPU ---------------------
\ ---------------------------------------------

hex
VARIABLE STATEM 10 ALLOT \ -------- Actual State
VARIABLE KEYM 10 ALLOT \ ------------------- Key
VARIABLE RKEY 10 ALLOT \ ------------ Round Key
VARIABLE BT \ ----------------------- Byte Temp
VARIABLE WT 4 ALLOT \ ----------- AES-Word Temp
VARIABLE MC 4 ALLOT \ -------- Mix-Columns Temp
VARIABLE ROUND \ ---------------- Round Counter

\ ----------- Lookup Tables -------------------

CREATE SBOX
63 , 7C , 77 , 7B , F2 , 6B , 6F , C5 , 
30 , 01 , 67 , 2B , FE , D7 , AB , 76 , 
CA , 82 , C9 , 7D , FA , 59 , 47 , F0 , 
AD , D4 , A2 , AF , 9C , A4 , 72 , C0 , 
B7 , FD , 93 , 26 , 36 , 3F , F7 , CC , 
34 , A5 , E5 , F1 , 71 , D8 , 31 , 15 , 
04 , C7 , 23 , C3 , 18 , 96 , 05 , 9A , 
07 , 12 , 80 , E2 , EB , 27 , B2 , 75 , 
09 , 83 , 2C , 1A , 1B , 6E , 5A , A0 ,
52 , 3B , D6 , B3 , 29 , E3 , 2F , 84 , 
53 , D1 , 00 , ED , 20 , FC , B1 , 5B , 
6A , CB , BE , 39 , 4A , 4C , 58 , CF , 
D0 , EF , AA , FB , 43 , 4D , 33 , 85 , 
45 , F9 , 02 , 7F , 50 , 3C , 9F , A8 , 
51 , A3 , 40 , 8F , 92 , 9D , 38 , F5 , 
BC , B6 , DA , 21 , 10 , FF , F3 , D2 , 
CD , 0C , 13 , EC , 5F , 97 , 44 , 17 , 
C4 , A7 , 7E , 3D , 64 , 5D , 19 , 73 , 
60 , 81 , 4F , DC , 22 , 2A , 90 , 88 , 
46 , EE , B8 , 14 , DE , 5E , 0B , DB , 
E0 , 32 , 3A , 0A , 49 , 06 , 24 , 5C , 
C2 , D3 , AC , 62 , 91 , 95 , E4 , 79 , 
E7 , C8 , 37 , 6D , 8D , D5 , 4E , A9 , 
6C , 56 , F4 , EA , 65 , 7A , AE , 08 , 
BA , 78 , 25 , 2E , 1C , A6 , B4 , C6 , 
E8 , DD , 74 , 1F , 4B , BD , 8B , 8A , 
70 , 3E , B5 , 66 , 48 , 03 , F6 , 0E , 
61 , 35 , 57 , B9 , 86 , C1 , 1D , 9E , 
E1 , F8 , 98 , 11 , 69 , D9 , 8E , 94 , 
9B , 1E , 87 , E9 , CE , 55 , 28 , DF , 
8C , A1 , 89 , 0D , BF , E6 , 42 , 68 , 
41 , 99 , 2D , 0F , B0 , 54 , BB , 16 ,

CREATE SBOXINV
52 , 09 , 6A , D5 , 30 , 36 , A5 , 38 , 
BF , 40 , A3 , 9E , 81 , F3 , D7 , FB , 
7C , E3 , 39 , 82 , 9B , 2F , FF , 87 , 
34 , 8E , 43 , 44 , C4 , DE , E9 , CB , 
54 , 7B , 94 , 32 , A6 , C2 , 23 , 3D , 
EE , 4C , 95 , 0B , 42 , FA , C3 , 4E , 
08 , 2E , A1 , 66 , 28 , D9 , 24 , B2 , 
76 , 5B , A2 , 49 , 6D , 8B , D1 , 25 , 
72 , F8 , F6 , 64 , 86 , 68 , 98 , 16 , 
D4 , A4 , 5C , CC , 5D , 65 , B6 , 92 , 
6C , 70 , 48 , 50 , FD , ED , B9 , DA , 
5E , 15 , 46 , 57 , A7 , 8D , 9D , 84 , 
90 , D8 , AB , 00 , 8C , BC , D3 , 0A , 
F7 , E4 , 58 , 05 , B8 , B3 , 45 , 06 , 
D0 , 2C , 1E , 8F , CA , 3F , 0F , 02 , 
C1 , AF , BD , 03 , 01 , 13 , 8A , 6B , 
3A , 91 , 11 , 41 , 4F , 67 , DC , EA , 
97 , F2 , CF , CE , F0 , B4 , E6 , 73 , 
96 , AC , 74 , 22 , E7 , AD , 35 , 85 , 
E2 , F9 , 37 , E8 , 1C , 75 , DF , 6E , 
47 , F1 , 1A , 71 , 1D , 29 , C5 , 89 , 
6F , B7 , 62 , 0E , AA , 18 , BE , 1B , 
FC , 56 , 3E , 4B , C6 , D2 , 79 , 20 , 
9A , DB , C0 , FE , 78 , CD , 5A , F4 , 
1F , DD , A8 , 33 , 88 , 07 , C7 , 31 , 
B1 , 12 , 10 , 59 , 27 , 80 , EC , 5F , 
60 , 51 , 7F , A9 , 19 , B5 , 4A , 0D , 
2D , E5 , 7A , 9F , 93 , C9 , 9C , EF , 
A0 , E0 , 3B , 4D , AE , 2A , F5 , B0 , 
C8 , EB , BB , 3C , 83 , 53 , 99 , 61 , 
17 , 2B , 04 , 7E , BA , 77 , D6 , 26 , 
E1 , 69 , 14 , 63 , 55 , 21 , 0C , 7D ,

CREATE LOGM
00 , 00 , 19 , 01 , 32 , 02 , 1a , c6 , 
4b , c7 , 1b , 68 , 33 , ee , df , 03 ,
64 , 04 , e0 , 0e , 34 , 8d , 81 , ef , 
4c , 71 , 08 , c8 , f8 , 69 , 1c , c1 ,
7d , c2 , 1d , b5 , f9 , b9 , 27 , 6a , 
4d , e4 , a6 , 72 , 9a , c9 , 09 , 78 ,
65 , 2f , 8a , 05 , 21 , 0f , e1 , 24 , 
12 , f0 , 82 , 45 , 35 , 93 , da , 8e ,
96 , 8f , db , bd , 36 , d0 , ce , 94 , 
13 , 5c , d2 , f1 , 40 , 46 , 83 , 38 ,
66 , dd , fd , 30 , bf , 06 , 8b , 62 , 
b3 , 25 , e2 , 98 , 22 , 88 , 91 , 10 ,
7e , 6e , 48 , c3 , a3 , b6 , 1e , 42 , 
3a , 6b , 28 , 54 , fa , 85 , 3d , ba ,
2b , 79 , 0a , 15 , 9b , 9f , 5e , ca , 
4e , d4 , ac , e5 , f3 , 73 , a7 , 57 ,
af , 58 , a8 , 50 , f4 , ea , d6 , 74 , 
4f , ae , e9 , d5 , e7 , e6 , ad , e8 ,
2c , d7 , 75 , 7a , eb , 16 , 0b , f5 , 
59 , cb , 5f , b0 , 9c , a9 , 51 , a0 ,
7f , 0c , f6 , 6f , 17 , c4 , 49 , ec , 
d8 , 43 , 1f , 2d , a4 , 76 , 7b , b7 ,
cc , bb , 3e , 5a , fb , 60 , b1 , 86 , 
3b , 52 , a1 , 6c , aa , 55 , 29 , 9d ,
97 , b2 , 87 , 90 , 61 , be , dc , fc , 
bc , 95 , cf , cd , 37 , 3f , 5b , d1 ,
53 , 39 , 84 , 3c , 41 , a2 , 6d , 47 , 
14 , 2a , 9e , 5d , 56 , f2 , d3 , ab ,
44 , 11 , 92 , d9 , 23 , 20 , 2e , 89 , 
b4 , 7c , b8 , 26 , 77 , 99 , e3 , a5 ,
67 , 4a , ed , de , c5 , 31 , fe , 18 , 
0d , 63 , 8c , 80 , c0 , f7 , 70 , 07 ,

CREATE LOGINV
01 , 03 , 05 , 0f , 11 , 33 , 55 , ff , 
1a , 2e , 72 , 96 , a1 , f8 , 13 , 35 ,
5f , e1 , 38 , 48 , d8 , 73 , 95 , a4 , 
f7 , 02 , 06 , 0a , 1e , 22 , 66 , aa ,
e5 , 34 , 5c , e4 , 37 , 59 , eb , 26 , 
6a , be , d9 , 70 , 90 , ab , e6 , 31 ,
53 , f5 , 04 , 0c , 14 , 3c , 44 , cc , 
4f , d1 , 68 , b8 , d3 , 6e , b2 , cd ,
4c , d4 , 67 , a9 , e0 , 3b , 4d , d7 , 
62 , a6 , f1 , 08 , 18 , 28 , 78 , 88 ,
83 , 9e , b9 , d0 , 6b , bd , dc , 7f , 
81 , 98 , b3 , ce , 49 , db , 76 , 9a , 
b5 , c4 , 57 , f9 , 10 , 30 , 50 , f0 , 
0b , 1d , 27 , 69 , bb , d6 , 61 , a3 ,
fe , 19 , 2b , 7d , 87 , 92 , ad , ec , 
2f , 71 , 93 , ae , e9 , 20 , 60 , a0 , 
fb , 16 , 3a , 4e , d2 , 6d , b7 , c2 , 
5d , e7 , 32 , 56 , fa , 15 , 3f , 41 , 
c3 , 5e , e2 , 3d , 47 , c9 , 40 , c0 , 
5b , ed , 2c , 74 , 9c , bf , da , 75 ,
9f , ba , d5 , 64 , ac , ef , 2a , 7e , 
82 , 9d , bc , df , 7a , 8e , 89 , 80 ,
9b , b6 , c1 , 58 , e8 , 23 , 65 , af , 
ea , 25 , 6f , b1 , c8 , 43 , c5 , 54 , 
fc , 1f , 21 , 63 , a5 , f4 , 07 , 09 , 
1b , 2d , 77 , 99 , b0 , cb , 46 , ca , 
45 , cf , 4a , de , 79 , 8b , 86 , 91 , 
a8 , e3 , 3e , 42 , c6 , 51 , f3 , 0e ,
12 , 36 , 5a , ee , 29 , 7b , 8d , 8c , 
8f , 8a , 85 , 94 , a7 , f2 , 0d , 17 , 
39 , 4b , dd , 7c , 84 , 97 , a2 , fd , 
1c , 24 , 6c , b4 , c7 , 52 , f6 , 01 ,

CREATE ROUNDCON 
00 , 01 , 02 , 04 , 08 , 10 , 20 , 40 , 80 , 1B , 36 ,

\ ----------- Helper words --------------------

: STATE@ ( n --- @ state + n )
  STATEM + @ ;

: STATE! ( n addrn --- ! state + n )
  STATEM + ! ;

: STATE-INIT ( --- state zeros )
  STATEM 10 0 FILL ;

: STATE-SET-LOW ( n0, .. , n7 --- state low 8 bytes )
  8 0 DO I 8 + 
    STATE! 
  LOOP ;

: STATE-SET-HIGH ( n0, .. , n7 --- state high 8 bytes )
  8 0 DO I 
    STATE! 
  LOOP ;

: KEY@ ( n --- @ key + n )
  KEYM + @ ;

: KEY! ( n addrn --- ! key + n )
  KEYM + ! ;

: KEY-INIT ( --- key zeros )
  KEYM 10 0 FILL ;

: KEY-SET-LOW ( n0, .. , n7 --- key low 8 bytes )
  8 0 DO I 8 +
    KEY!
  LOOP ;

: KEY-SET-HIGH ( n0, .. , n7 --- key high 8 bytes )
  8 0 DO I
    KEY!
  LOOP ;

: RKEY@ ( n --- @ rkey + n )
  RKEY + @ ;

: RKEY! ( n addrn --- ! rkey + n )
  RKEY + ! ;

: RKEY-INIT ( --- key zeros )
  RKEY 10 0 FILL ;

: WT@ ( n --- @ wt + n )
  WT + @ ;

: WT! ( n addrn --- ! wt + n )
  WT + ! ;

: WT-INIT
  WT 4 0  FILL ;

: MC@ ( n --- @ mc + n )
  MC + @ ;

: MC! ( n addrn --- ! mc + n )
  MC + ! ;

: MC-INIT
  MC 4 0 FILL ;

: SBOX@ ( n --- @ sbox + n )
  SBOX + @ ;

: SBOXINV@ ( n --- @ sboxinv + n )
  SBOXINV + @ ;

: LOG@
  LOGM + @ ;

: LOGINV@
  LOGINV + @ ;


: MULGF2 ( n1,n2 --- n )
  OVER IF 
    LOG@ SWAP LOG@ +
    FF MOD LOGINV@
  ELSE 
    DROP DROP 0 
  THEN ;

\ --- Ouptut words ----------------------------

: STATE? ( --- )
  10 0 DO I 
    STATE@ .x2 
  LOOP ;

: KEY? ( --- )
  10 0 DO I 
    KEY@ .x2 
  LOOP ;

: RKEY? ( --- )
  10 0 DO I 
    RKEY@ .x2 
  LOOP ;

: WT? ( --- )
  4 0 DO I 
    WT@ .x2 
  LOOP ;

\ --- Key Expansion ---------------------------

: ROTWORD ( wt --- wt )
  0 WT@ BT ! 1 WT@ 0 WT! 2 WT@ 1 WT! 3 WT@ 2 WT! BT @ 3 WT! ;

: SUBWORD ( wt --- wt )
  4 0 DO I WT@ SBOX@ I WT! LOOP ;

: RCON ( wt --- wt )
  0 WT@ ROUND @ ROUNDCON + @ XOR 0 WT! ;

: NEXTKEY ( rkey --- next rkey )
  \ --- word 0
  4 0 DO I C + RKEY@ I WT! LOOP
  ROTWORD SUBWORD RCON
  4 0 DO I RKEY@ I WT@ XOR I RKEY! LOOP
  \ --- word 1
  4 0 DO I 4 + RKEY@ I RKEY@ XOR I 4 + RKEY! LOOP
  \ --- word 2
  4 0 DO I 8 + RKEY@ I 4 + RKEY@ XOR I 8 + RKEY! LOOP
  \ --- word 3
  4 0 DO I C + RKEY@ I 8 + RKEY@ XOR I C + RKEY! LOOP ;
\ ---------------------------------------------
\ --------- AES 128 INIT ----------------------
\ ---------------------------------------------

: AES-INIT
  STATE-INIT
  KEY-INIT
  RKEY-INIT
  WT-INIT
  MC-INIT ;

\ ---------------------------------------------
\ --------- AES 128 ENCODING ------------------
\ ---------------------------------------------

: BYTES-SBOX ( state --- state )
  10 0 DO 
    I STATE@ SBOX@ 
    I STATE! 
  LOOP ;

: SHIFT-ROWS ( state --- state )
  1 STATE@ BT ! 5 STATE@ 1 STATE! 9 STATE@ 5 STATE! D STATE@ 9 STATE! BT @ D STATE!
  2 STATE@ BT ! A STATE@ 2 STATE! BT @ A STATE! 6 STATE@ BT ! E STATE@ 6 STATE! BT @ E STATE! 
  F STATE@ BT ! B STATE@ F STATE! 7 STATE@ B STATE! 3 STATE@ 7 STATE! BT @ 3 STATE! ;

: MIX-COLUMNS
  D 0 DO 
    \ --- i+0. byte of column
    I STATE@ 2 MULGF2   
    I 1 + STATE@ 3 MULGF2 XOR
    I 2 + STATE@ XOR
    I 3 + STATE@ XOR
    0 MC!
    \ --- i+1. byte of column
    I STATE@
    I 1 + STATE@ 2 MULGF2 XOR
    I 2 + STATE@ 3 MULGF2 XOR
    I 3 + STATE@ XOR
    1 MC!
    \ --- i+2. byte of column
    I STATE@
    I 1 + STATE@ XOR
    I 2 + STATE@ 2 MULGF2 XOR
    I 3 + STATE@ 3 MULGF2 XOR
    2 MC!
    \ --- i+3. byte of column
    I STATE@ 3 MULGF2
    I 1 + STATE@ XOR
    I 2 + STATE@ XOR
    I 3 + STATE@ 2 MULGF2 XOR
    3 MC!
    4 0 DO
      I MC@
      I J + STATE!
    LOOP
  4 +LOOP ;

: ADD-RKEY0 ( key --- rkey , state --- state)
  10 0 DO
    I KEY@
    I RKEY!
  LOOP
  10 0 DO
    I STATE@
    I RKEY@
    XOR
    I STATE!
  LOOP ;

: ADD-RKEYN
  NEXTKEY
  10 0 DO
    I STATE@
    I RKEY@
    XOR
    I STATE!
  LOOP ;

: ENCODE128 ( key plainstate --- cipherstate )
  0 ROUND C! 
  ADD-RKEY0
  A 1 DO 
    I ROUND C!
    BYTES-SBOX
    SHIFT-ROWS
    MIX-COLUMNS
    ADD-RKEYN
  LOOP
  A ROUND C!
  BYTES-SBOX
  SHIFT-ROWS
  ADD-RKEYN ;

\ ---------------------------------------------
\ --------- AES 128 DECODING ------------------
\ ---------------------------------------------

: BYTES-SBOX-INV
  10 0 DO
    I STATE@ SBOXINV@
    I STATE!
  LOOP ;

: SHIFT-ROWS-INV
  ;

: MIX-COLUMNS-INV
  ;

: ADD-RKEY-INV
  ;

: DECODE128
  ;

\ --------- TESTING ---------------------------

: SET-VARS

  AES-INIT

  \ ---- Key from FIPS-197 ( key-byte nr. 0 on top of the stack )
  0F 0E 0D 0C 0B 0A 09 08 
  KEY-SET-LOW
  07 06 05 04 03 02 01 00
  KEY-SET-HIGH

  \ ---- Plaintext from FIPS-197 ( state-byte nr. 0 on top of the stack)
  FF EE DD CC BB AA 99 88 
  STATE-SET-LOW
  77 66 55 44 33 22 11 00
  STATE-SET-HIGH ;

: ENCTEST

  SET-VARS
  \ ---- Do thwe encoding
  CR ." KEY:    " KEY?
  CR ." PLAIN:  " STATE?
  ENCODE128
  CR ." ENCODING... "
  CR ." CIPHER: " STATE? ;

