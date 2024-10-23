# Address Translation

## Some pre reqs

1. What is MIPS. Its a [computer architecture. Microprocessor without Interlocked Pipelined Stages](https://en.wikipedia.org/wiki/MIPS_architecture)

2. [Instruction set architecture or ISA] is an abstract model that generally defines how software controls the CPU in a computer. MIPS is an ISA.

## Notes
1. Here we go.

2. asdf

3. asdfsadf

## Reference
1. https://www.youtube.com/watch?v=KNUJhZCQZ9c
2. https://www.youtube.com/watch?v=KNUJhZCQZ9c&list=PLiwt1iVUib9s2Uo5BeYmwkDFUh70fJPxX&index=5

## Some basic Math for Memory calcs

| Two | Power    |  In Words                   | Value                | Kb/Mb/Gb  |
| :-- | :--------| :--------------------------:| :--------------------|-----------|
| 2	  | 1	     |  2 raised to power 1	       | 2                     |           |
| 2	  | 2	     |  2 raised to power 2	       | 4                     |           |
| 2	  | 3	     |  2 raised to power 3	       | 8                     |           |
| 2	  | 4	     |  2 raised to power 4	       | 16	                   |           |
| 2	  | 5	     |  2 raised to power 5	       | 32	                   |           |
| 2	  | 6	     |  2 raised to power 6	       | 64	                   |           |
| 2	  | 7	     |  2 raised to power 7	       | 128	               |           |
| 2	  | 8	     |  2 raised to power 8	       | 256                   |           |
| 2	  | 9	     |  2 raised to power 9	       | 512                   |           |
| 2	  | 10       |	2 raised to power 10       | 1024                  |   1KB     |
| 2	  | 11       |	2 raised to power 11       | 2048                  |           |
| 2	  | 12       |	2 raised to power 12       | 4096                  |           |
| 2	  | 13       |	2 raised to power 13       | 8192                  |           |
| 2	  | 14       |	2 raised to power 14       | 16384                 |           |
| 2	  | 15       |	2 raised to power 15       | 32768                 |           |
| 2	  | 16       |	2 raised to power 16       | 65536                 |           |
| 2	  | 17       |	2 raised to power 17       | 131072                |           |
| 2	  | 18       |	2 raised to power 18       | 262144                |           |
| 2	  | 19       |	2 raised to power 19       | 524288                |           |
| 2	  | 20       |	2 raised to power 20       | 1048576               |   1MB     |
| 2	  | 21       |	2 raised to power 21       | 2097152               |   2MB     |
| 2	  | 22       |	2 raised to power 22       | 4194304               |   4MB     |
| 2	  | 23       |	2 raised to power 23       | 8388608               |   8MB     |
| 2	  | 24       |	2 raised to power 24       | 16777216              |   16MB    |
| 2	  | 25       |	2 raised to power 25       | 33554432              |   32MB    |
| 2	  | 26       |	2 raised to power 26       | 67108864              |   64MB    |
| 2	  | 27       |	2 raised to power 27       | 134217728             |   128MB   | 
| 2	  | 28       |	2 raised to power 28       | 268435456             |   256MB   | 
| 2	  | 29       |	2 raised to power 29       | 536870912             |           | 
| 2	  | 30       |	2 raised to power 30       | 1073741824            |   1GB     | 1 Billion 
| 2	  | 31       |	2 raised to power 31       | 2147483648            |   2GB     |
| 2	  | 32       |	2 raised to power 32       | 4294967296            |   4GB     | 
