Get the aes to work:

1. Boot up the J1
2. Get a connection to the shell.py on your PC
3. #include swapforth.fs
4. #include aes.fs
5. enctest

Notes:

- The code is experimental, don't use it productive.
- It's work-in-progress and not nearly finished.
- The sbox- and sboxinv-Lookup-Tables are from FIPS-197.
- The log- and loginv-Lookup-Tables are selfcalculated.
- The test-vectors are from FIPS-197 too.


