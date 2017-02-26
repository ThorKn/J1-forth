Get the aes to work:

1. Boot up the J1 with swapforth (#include swapforth.fs)
2. Get a connection to the shell.py on your PC
3. #include aes.fs
4. enctest

Notes:

- The code is experimental, don't use it productive.
- It's work-in-progress and not nearly finished.
- The sbox-, sboxinv-, log- and loginv-Lookup-Tables are from FIPS-197.
- The test-vectors are from FIPS-197 too.


