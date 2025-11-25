INCLUDE globals.ink

{ gamePhase:
- 0: //tutorial
- 0.1: -> lunch
- 1: -> phase1
- 1.1: -> phase1check
- 2: //phase2
- 3: //phase3
}

=== lunch ===
Anton, Mahal! It's time for lunch. #speaker:Rosalyn #portrait:rosalyn
I'm here. #speaker:Anton #portrait:anton
Hi, Pa. #speaker:Benji #portrait:benji
Good evening, po. #speaker:L??? #portrait:liza
What's for lunch? #speaker:Anton #portrait:anton
Pork Adobo. Courtesy of our dear Liza. #speaker:Rosalyn #portrait:rosalyn
Hm. #speaker:Anton #portrait:anton
I hope you enjoy it. #speaker:Liza #portrait:liza
I hope you didn't strain yourself too much. You know what your midwife said. #speaker:Ben?i #portrait:benji
It's just some chopping and then letting it stew, Ben. #speaker:L?za #portrait:liza
Nothing too serious. 
I'm just worried. You have to be careful. #speaker:Ben?? #portrait:benji
Oh, you worry too much, Benji! #speaker:Rosalyn #portrait:rosalyn
She can handle herself. 
She's only 4 months in. 
When I was pregnant with you, 
I was up and about the whole way until you were born!
Isn't that right, dear?
+ I don't remember. #speaker:Anton #portrait:anton
    ... #speaker:Rosalyn #portrait:rosalyn
    That's okay, Mahal. It's been a long time anyway.
+ She's right. #speaker:Anton #portrait:anton
    She used to run Barangay Calesa with an iron fist like she was never pregnant at all! #speaker:Anton #portrait:anton
    Walking around like she owns the place.
    Scolding the local ruffians.
    Gossiping with the neighbors.
    Speaking of which, what's going on with the neighborhood nowadays?
    You haven't been doing your daily patrols around the Barangay as of late.
    ... #speaker:Rosalyn #portrait:rosalyn
    ... #speaker:Benj? #portrait:benji
    Mahal, we don't live in Barangay Calesa anymore, remember? #speaker:Rosalyn #portrait:rosalyn
    ...Huh? #speaker:Anton #portrait:anton
- 
\(sighs) It's okay, Pa. Don't worry about it too much. #speaker:Benj? #portrait:benji
Let's just eat, okay? #speaker:Rosalyn #portrait:rosalyn
...Okay #speaker:Anton #portrait:anton
... #speaker:Benji #portrait:benji
... #speaker:Li?a #portrait:liza
... #speaker:Rosalyn #portrait:rosalyn
... #speaker:Anton #portrait:anton

...Liza and I plan to push through with the move. #speaker:Benj? #portrait:benji
Oh. #speaker:Rosalyn #portrait:rosalyn
...Where are you moving to? #speaker:Anton #portrait:anton
Closer to the city. About 40 minutes from here. #speaker:Benji #portrait:benji
I'll write the address down later.
...
Ma, are you sure you don't want a caretaker?
I'm sure. #speaker:Rosalyn #portrait:rosalyn
I can take care of your father myself.
I can take care of myself too. #speaker:Anton #portrait:anton
I'm sure you both can. #speaker:Be?ji #portrait:benji
But maybe you can get someone to help around the house.
Just someone to help with cooking and cleaning. #speaker:Li?a #portrait:rosalyn
Since Benji and I won't be around as much anymore.
We're worried about you. #speaker:Be?ji #portrait:benji
We'll be fine. #speaker:Anton #portrait:anton
It just means you have to visit often! #speaker:Rosalyn #portrait:rosalyn
\(chuckles) We will. #speaker:Benji #portrait:benji
Come now. Let's finish lunch. #speaker:Rosalyn #portrait:rosalyn
~ gamePhase = 1
-> DONE


=== phase1 ===
Hm. This house is getting old. #speaker:Anton #portrait:anton
I know, Mahal. That's why I'm redecorating! #speaker:Rosalyn #portrait:rosalyn
Do you like the color?
+ Not really.
    Well, that's too bad. I already bought the paints.
+ Yeah.
    Me too! I'm glad you like it.
- 
Oh, shoot. I'm almost out of paint.
I swear I bought another bucket.
Mahal, can you be a dear and look for it? I'll just finish this up.
+ [Why should I?]
    Why not look for it yourself? #speaker:Anton #portrait:anton
    Don't give me that attitude! Just go look for it, please? #speaker:Rosalyn #portrait:rosalyn
    Fine. #speaker:Anton #portrait:anton
+ [Okay.]
    Will do.#speaker:Anton #portrait:anton
-
Thank you, Anton. #speaker:Rosalyn #portrait:rosalyn
Oh, can you find a paint roller and a small brush while you're at it? I might need them.
// go to phase1check
~ gamePhase = 1.1
-> DONE


=== phase1check ===
Did you find the stuff? #speaker:Rosalyn #portrait:rosalyn
+ Not yet. #speaker:Anton #portrait:anton
    Let me know when you do. #speaker:Rosalyn #portrait:rosalyn
+ Yeah, I have them. #speaker:Anton #portrait:anton
    { phase1ItemsComplete: 
        -> phase1complete
    - else:
        Haha, very funny. Let me know when you actually get them. #speaker:Rosalyn #portrait:rosalyn
    }
-
-> DONE

=== phase1complete ===
Thank you, Anton. #speaker:Rosalyn #portrait:rosalyn
I painted these doors for you. So that you don't get lost again.
Now you'll know which room is which.
Thank you, Rosa. #speaker:Anton #portrait:anton
Of course, Mahal. #speaker:Rosalyn #portrait:rosalyn
// complete signal here
~gamePhase = 2
-> DONE


