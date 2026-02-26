INCLUDE globals.ink

{ gamePhase:
- 0: -> lunch
- 0.1: //
- 1: -> phase1
- 1.1: -> phase1check
- 2: -> phase2start
- 2.1: -> phase2GetDishSoap
- 2.2: -> phase2DishSoapReceived
- 2.3: -> phase2GetTrashBags
- 2.4: -> phase2TrashBagsReceived
- 3: -> phase3start
- 3.1: -> phase3GetTape
- 3.2: -> phase3TapeReceived
- 4: -> phase4
}

=== lunch ===
Anton, Mahal! It's time for lunch. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
I'm here. #speaker:Anton #portrait:Character_Anton_Neutral
Hi, Pa. #speaker:Benji #portrait:Character_Benji_Neutral
Good evening, po. #speaker:L??? #portrait:Character__Liza_Neutral
What's for lunch? #speaker:Anton #portrait:Character_Anton_Neutral
Pork Adobo. Courtesy of our dear Liza. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
Hm. #speaker:Anton #portrait:Character_Anton_Neutral
I hope you enjoy it. #speaker:Liza #portrait:Character_Liza_Neutral
I hope you didn't strain yourself too much. You know what your midwife said. #speaker:Ben?i #portrait:Character_Benji_Sad
It's just some chopping and then letting it stew, Ben. #speaker:L?za #portrait:Character_Liza_Neutral
Nothing too serious. 
I'm just worried. You have to be careful. #speaker:Ben?? #portrait:Character_Benji_Sad
Oh, you worry too much, Benji! #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
She can handle herself. 
She's only 4 months in. 
When I was pregnant with you, 
I was up and about the whole way until you were born!
Isn't that right, dear?
+ [I forgot.]
    I... #speaker:Anton #portrait:Character_Anton_Sad
    I don't remember. 
    ... #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
    That's okay, Mahal. It's been a long time anyway.
+ She's right. #speaker:Anton #portrait:Character_Anton_Neutral
    She used to run Barangay Calesa with an iron fist like she was never pregnant at all!
    Walking around like she owns the place.
    Scolding the local ruffians.
    Gossiping with the neighbors.
    Speaking of which, what's going on with the neighborhood nowadays?
    You haven't been doing your daily patrols around the Barangay as of late.
    ... #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
    ... #speaker:Benj? #portrait:Character_Benji_Sad
    ..? #speaker:L??a #portrait:Character_Liza_Neutral
    Mahal, we don't live in Barangay Calesa anymore, remember? #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
    ...Huh? #speaker:Anton #portrait:Character_Anton_Sad
- 
\(sighs) It's okay, Pa. Don't worry about it too much. #speaker:Benj? #portrait:Character_Benji_Sad
Let's just eat, okay? #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
...Okay #speaker:Anton #portrait:Character_Anton_Sad
... #speaker:Benji #portrait:Character_Benji_Sad
... #speaker:Li?a #portrait:Character_Liza_Neutral
... #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
... #speaker:Anton #portrait:Character_Anton_Sad
...Liza and I plan to push through with the move. #speaker:Benj? #portrait:Character_Benji_Neutral
Oh. #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
...Where are you moving to? #speaker:Anton #portrait:Character_Anton_Sad
Closer to the city. About an hour from here. #speaker:Benji #portrait:Character_Benji_Neutral
I'll write the address down later.
...
Ma, are you sure you don't want a caretaker?
I'm sure. #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
I can take care of your father myself.
I can take care of myself too. #speaker:Anton #portrait:Character_Anton_Neutral
I'm sure you both can. #speaker:Be?ji #portrait:Character_Benji_Neutral
But maybe you can get someone to help around the house.
Maybe someone to help with cooking and cleaning. #speaker:Li?a #portrait:Character_Liza_Neutral
Since Benji and I won't be around as much anymore.
We're worried about you. #speaker:Be?ji #portrait:Character_Benji_Neutral
We'll be fine. #speaker:Anton #portrait:Character_Anton_Neutral
It just means you have to visit often! #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
\(chuckles) We will. #speaker:Benji #portrait:Character_Benji_Happy
Come now. Let's finish lunch. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
~ gamePhase = 1
-> DONE


=== phase1 ===
Hm. This house is getting old. #speaker:Anton #portrait:Character_Anton_Neutral
I know, Mahal. That's why I'm redecorating! #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
Do you like the color?
+ [No]
    Not really. #speaker:Anton #portrait:Character_Anton_Neutral
    How blunt! #speaker:Rosalyn #portrait:Character_Rosalyn_Angry
    You could have at least lied and pretended to like it!
    Well, that's too bad. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
    I already bought the paints.
+ Yeah. #speaker:Anton #portrait:Character_Anton_Neutral
    Me too! #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
    I'm glad you like it. 
- 
Oh, shoot. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
I'm almost out of paint.
I swear I bought another bucket.
Mahal, can you be a dear and look for it? I'll just finish this up.
+ [Why should I?]
    Why not look for it yourself? #speaker:Anton #portrait:Character_Anton_Neutral
    Don't give me that attitude! #speaker:Rosalyn #portrait:Character_Rosalyn_Angry
    Just go look for it, please? #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
    Fine. #speaker:Anton #portrait:Character_Anton_Neutral
+ [Okay.]
    Will do.#speaker:Anton #portrait:Character_Anton_Neutral
-
Thank you, Anton. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
Oh, can you find a paint roller and a small brush while you're at it? I might need them.
// go to phase1check
~ gamePhase = 1.1
-> DONE


=== phase1check ===
Did you find the stuff? #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
+ Not yet. #speaker:Anton #portrait:Character_Anton_Neutral
    Let me know when you do. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
+ [Yeah.] 
    Yeah, I have them. #speaker:Anton #portrait:Character_Anton_Neutral
    { p1q1s1done and p1q1d1done: 
        -> phase1complete
    - else:
        Haha, very funny. Let me know when you actually get them. #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
    }
-
-> DONE

=== phase1complete ===
Thank you, Anton. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
I painted these doors for you. 
So that you don't get lost again.
Now you'll know which room is which.
... #speaker:Anton #portrait:Character_Anton_Neutral
Thank you, Rosa. 
Of course, Mahal. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
// complete signal here
~gamePhase = 2
-> DONE

=== phase2start ===
She seems busy. #speaker:Anton portrait:Character_Anton_Neutral
-> DONE

=== phase2GetDishSoap ===
Rosa. #speaker:Anton #portrait:Character_Anton_Neutral
Yes, Mahal? #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
Do you remember where our dish soap is? #speaker:Anton #portrait:Character_Anton_Neutral
Dish soap? #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
Oh! 
We hadn't packed the groceries.
It should be...
Yup! #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
It's right here.
Here you go.
Thank you. #speaker:Anton #portrait:Character_Anton_Neutral
...
+ [Painting?]
    Why did you start painting again? #speaker:Anton #portrait:Character_Anton_Neutral
    I wanted to get back into it. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
    Little by little.
    It's lovely. #speaker:Anton #portrait:Character_Anton_Neutral
    Thank you! #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
    Just... #speaker:Anton #portrait:Character_Anton_Neutral
    Why doors?
    Well... #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
    Why not?
    It's a...
    Special project.
    And I really hope you'll like it.
    ? #speaker:Anton #portrait:Character_Anton_Neutral
    I just said it's lovely didn't I?
    \(chuckles) #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
    I know, Mahal.
    But,
    I hope you'll like it for other reasons...
    What are you on about? #speaker:Anton #portrait:Character_Anton_Neutral
    I'll tell you all about it later when I'm finished. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
    Mkay?
    ... #speaker:Anton #portrait:Character_Anton_Neutral
    Fine.
    And what about you, Mahal? #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
    Such a chatterbox all of a sudden!
    Hmp. #speaker:Anton #portrait:Character_Anton_Neutral
    If you wanted me to shut up, you could have just said so.
    You know that's not what I meant! #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
    I like hearing you talk. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
    I much prefer you being all chatty.
    At least, compared to the silence in the house... #speaker:Rosalyn #portraitCharacter_Rosalyn_Sad
    Hm. #speaker:Anton #portrait:Character_Anton_Neutral
+ [Locks?]
    What's with all the locks on the door? #speaker:Anton #portrait:Character_Anton_Neutral
    Ah... #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
    Those are...
    For your sake...
    ... #speaker:Anton #portrait:Character_Anton_Neutral
    What?
    I um- #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
    I read online that some Alzheimer's patients.
    Have a tendency to wander out of the house.
    Without knowing where they are.
    Or how to get back.
    So...
    Locks.
    Of course I would know how to get back to my own house! #speaker:Anton #portrait:Character_Anton_Angry
    How long do you think we've been living here?!
    I'm sure you can, Mahal! #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
    This is just...
    A precaution...
    Because you don't trust me with myself. #speaker:Anton #portrait:Character_Anton_Neutral
    Mahal, it's not like that. #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
    Sure it isn't. #speaker:Anton #portrait:Character_Anton_Neutral
    Anton. #speaker:Rosalyn #portrait:Character_Rosa_Sad
    ... #speaker:Anton #portrait:Character_Anton_Neutral
    ... #speaker:Rosalyn #portrait:Character_Rosa_Sad
    You think I'm a lost cause, don't you? #speaker:Anton #portrait:Character_Anton_Neutral
    Not at all. #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
    I just want to make sure you're safe.
    By keeping me locked up in the house? #speaker:Anton #portrait:Character_Anton_Neutral
    By making sure we know where you are. #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
    ... #speaker:Anton #portrait:Character_Anton_Neutral
    I'm going now. #speaker:Anton #portrait:Character_Anton_Sad
    ... #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
    Okay. 
    Take care, Mahal.
-
Hm. #speaker:Anton #portrait:Character_Anton_Neutral
~ gamePhase = 2.2
-> DONE

=== phase2DishSoapReceived ===
She's back to painting. #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase2GetTrashBags ===
She seems busy. #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase2TrashBagsReceived ===
The door looks beautiful. #speaker:Anton #portrait:Character_Anton_Neutral
Aw! #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
Thank you, Mahal.
-> DONE

=== phase3start ===
Hello, Mahal. #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
...Hello. #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase3GetTape ===
Rosa? #speaker:Anton #portrait:Character_Anton_Neutral
Hm? #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
Oh. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
Yes, mahal?
We need some tape for the boxes. #speaker:Anton #portrait:Character_Anton_Neutral
Do you have any?
Tape? #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
Here you go.
Thank you. #speaker:Anton #portrait:Character_Anton_Neutral
You're welcome. #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
... #speaker:Anton #portrait:Character_Anton_Neutral
Mahal?
Are you...
Alright?
\(sigh) #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
I am.
I'm just...
Annoyed.
Liza keeps suggesting that we get a caregiver for you.
I know she means well.
But I can handle myself! #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
We can take care of ourselves!
We'll be okay.
Right, Mahal?
... #speaker:Anton #portrait:Character_Anton_Neutral
We will.
... #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
We will. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
Thank you, Mahal.
I love you.
I love you too. #speaker:Anton #portrait:Character_Anton_Neutral
Now... #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
Don't you need to give that tape to someone? 
Oh. #speaker:Anton #portrait:Character_Anton_Neutral
Right.
I'll go do that.
Thank you.
You're welcome, Mahal. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
~ gamePhase = 3.2
-> DONE

=== phase3TapeReceived ===
Did you give the tape yet? #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
Not yet. #speaker:Anton #portrait:Character_Anton_Neutral
Go do that, Mahal. #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
I'll be okay here. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
-> DONE

=== phase4 ===
It's almost time... #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
Rosa? #speaker:Anton #portrait:Character_Anton_Neutral
I'll be okay... #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
-> DONE