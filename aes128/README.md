Get the aes to work:

1. Boot up the J1 with swapforth (#include swapforth.fs)
2. Get a connection to the shell.py on your PC
3. #include aes.fs

Notes:

- The sbox-, sboxinv-, log- and loginv-Lookup-Tables are from NIST-197.
- The code is experimental, don't use it productive.
- The test-vectors are from NIST-197 too.


