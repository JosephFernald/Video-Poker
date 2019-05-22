Some notes on the Implementation of Video Poker:

1) I used a class from the unify wiki called Advanced C# messenger in order to decouple my code.  This allows for things to be moved to scriptable objects or prefabs and not break or have to be re-hooked up in the scene.

2) While I have worked in the slot gaming industry for 8 years I'm unfamiliar with the game poker, so I learned most of the rules from the Wikipedia entry as well as playing some online games.  While I attempted to be comprehensive in my coverage of things, it is possible I missed something that would be obvious to others who worked on video poker projects.

3) I intentionally avoided the use of Linq even though it could've simplified a number of the queries I was doing in order to evaluate the current hand, this was done in order to try and remove any unintentional memory allocations and boxing.

4) I sacrificed some performance in order to create classes/functions that would be more generic in their nature and be able to be re-used for other poker games implementations.   If the goal had been performance I would've created my own array/list/stack container that could do the job I needed but it would've been pooled, and spent more time digging into cache coherence issues.

5) I created the poker strategy interface class in order to allow for the addition of future poker variations.  Right now there is only 1 class inheriting from the interface but the intention was that there would eventually be more.

6) I did not use the Light Weight Rendering pipeline as it is still considered "Pre-release.