
## Notes
1. So now we're going to talk about virtual memory. 
2. Virtual memory is one of the coolest parts of computer architecture because it really combines the idea of trying to make the Software work well with what we can do in hardware.
3. So in talking about virtual memory we're going to start off with three problems. 
   1. The first of these problems is what happens if you don't have enough Ram in your computer. 
      1. This problem was really the motivation for virtual memory in the first place. 
      2. It used to be that RAM was really expensive and so you wanted to use it very efficiently and you need to be able to run programs Even if you didn't have enough Memory.
      3. Now these days we to have Tons of RAM on our Computers.
      4. So this is much less of a problem, in fact if You Run out of memory on a computer today and have to use virtual memory as a result it generally slows things down so much that it's almost not worth doing it. 
   2. The second problem. We have holes in our address space.
      1. If you run multiple programs together and you quit some, then you have chunks of Memory that are unavailable.
   3. We also have Programs running over each other. 
      1. Remember in MS we promised each program Access 32 bits address space. 
      2. If we have programs both write to address space of  1000 what happens? 
      3. Do they write over each other? How do we keep them from crashing each other?
   4.  What is virtual memory? 
       1. Its basically indirection. Virtual memory is the idea that we take the address the program uses, and we map it to the real address in memory. 
       2. Mapping Going Back and forth we can do a lot of interesting things about controlling where the memory goes and how we use it.
       3. This indirection solves the problems above.
       4. The idea of page tables and translation which is how we store The mappings and how we actually do the mappings.
       5. Then we are going to talk about how we implement virtual memory like 
          1. where do we store these page tables, 
          2. these mappings between our addresses and 
          3. how do we make them and run really fast.
          4. So we are going to make them run fast by adding hardware to the computer to make it faster. 
          5. And finally we are going to talk about how the virtual memory interacts with caches.

## References
1. https://www.youtube.com/watch?v=qcBIvnQt0Bw&list=PLiwt1iVUib9s2Uo5BeYmwkDFUh70fJPxX
2. https://www.youtube.com/watch?v=qcBIvnQt0Bw