
INCLUDE globals.ink

{ gamePhase:
- 0: -> lunch
- 1: -> phase1
- 1.1: -> phase1check
- 2: -> phase2start
- 2.1: -> phase2check
- 3: -> phase3start
- 3.1: -> phase3check
- 4: -> phase4
- 4.1: -> phase4
- 5: -> phase5
}

=== lunch ===
Anton, Mahal! It's time for lunch. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy #effect:vignette
I'm here. #speaker:Anton #portrait:Character_Anton_Neutral
Hi, Pa. #speaker:Benji #portrait:Character_Benji_Neutral
Good evening, po. #speaker:L??? #portrait:Character_Liza_Neutral
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
+ She's right. #speaker:Anton #portrait:Character_Anton_Neutral #effect:vignette
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
    ...Huh? #speaker:Anton #portrait:Character_Anton_Sad #effect:none
- 
\(sighs)  #speaker:Benj? #portrait:Character_Benji_Sad
It's okay, Pa. Don't worry about it too much.
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
\(chuckles)  #speaker:Benji #portrait:Character_Benji_Happy
We will.
Come now.  #speaker:Rosalyn #portrait:Character_Rosalyn_Happy #effect:none
Let's finish lunch.
~ gamePhase = 1
-> DONE


=== phase1 ===
Hm. #speaker:Anton #portrait:Character_Anton_Neutral
This house is getting old. 
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
Oh, can you find a paint roller and brushes while you're at it?
I might need them.
Thank you, Mahal.
// go to phase1check
~ gamePhase = 1.1
-> DONE


=== phase1check ===
Did you find the stuff? #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
+ Not yet. #speaker:Anton #portrait:Character_Anton_Neutral
    Let me know when you do. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
+ [Yeah.] 
    Yeah, I have them. #speaker:Anton #portrait:Character_Anton_Neutral
    { p1HaveBrush and p1HaveBucket and p1HaveRoller and p1SonDone and p1DILDone  : 
        -> phase1complete
    - else:
        Haha, very funny. #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
        Let me know when you actually get them. 
    }
-
-> DONE

=== phase1complete ===
Thank you, Mahal. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
How long will you be doing this? #speaker:Anton #portrait:Character_Anton_Neutral
Hm... #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
I want to paint every door in the house.
So it would take quite a while, I think. 
Would you like to help me? #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
Hmp. #speaker:Anton #portrait:Character_Anton_Neutral
You know I'm not good at those art things.
You're much better at it than I am.
I'll leave it to you.
Haha! #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
Thank you, Mahal.
~ gamePhase = 2
-> DONE

=== phase2start ===
She seems busy. #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase2check ===
{ p2HaveSoap and p2WifeDone:
    -> phase2DishSoapReceived
  - else:
    -> phase2GetDishSoap
}

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
    I wanted to get back into it. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy #effect:vignette
    Little by little.
    ... #speaker:Anton #portrait:Character_Anton_Neutral
    It's lovely. 
    Thank you! #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
    Just... #speaker:Anton #portrait:Character_Anton_Neutral
    Why doors?
    Well... #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
    Why not?
    It's a...
    Special project.
    And I really hope you'll like it.
    ...? #speaker:Anton #portrait:Character_Anton_Neutral
    I just said it's lovely didn't I?
    \(chuckles) #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
    I know, Mahal.
    But... #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
    I hope you'll like it for other reasons... #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
    What are you on about? #speaker:Anton #portrait:Character_Anton_Neutral
    I'll tell you all about it later when I'm finished. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
    Mkay?
    ... #speaker:Anton #portrait:Character_Anton_Neutral
    Fine.
    And what about you, Mahal? #speaker:Rosalyn #portrait:Character_Rosalyn_Happy #effect:none
    Such a chatterbox all of a sudden!
    Hmp. #speaker:Anton #portrait:Character_Anton_Neutral
    If you wanted me to shut up, you could have just said so.
    You know that's not what I meant! #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
    I like hearing you talk. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
    I much prefer you being all chatty.
    At least, compared to the silence in the house... #speaker:Rosalyn #portraitCharacter_Rosalyn_Sad
+ [Locks?]
    What's with all the locks on the door? #speaker:Anton #portrait:Character_Anton_Neutral
    Ah... #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
    Those are...
    For your sake...
    ... #speaker:Anton #portrait:Character_Anton_Neutral
    What? #effect:tense
    I um- #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
    I read online that some Alzheimer's patients.
    Have a tendency to wander out of the house.
    Without knowing where they are.
    Or how to get back.
    So...
    Locks.
    Of course I would know how to get back to my own house! #speaker:Anton #portrait:Character_Anton_Angry #effect:shake
    How long do you think we've been living here?!
    I'm sure you can, Mahal! #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
    This is just...
    A precaution...
    ... #speaker:Anton #portrait:Character_Anton_Neutral
    Because you don't trust me with myself. 
    Mahal, it's not like that. #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
    Sure it isn't. #speaker:Anton #portrait:Character_Anton_Neutral
    Anton. #speaker:Rosalyn #portrait:Character_Rosa_Sad
    ... #speaker:Anton #portrait:Character_Anton_Neutral
    ... #speaker:Rosalyn #portrait:Character_Rosa_Sad
    You think I'm a lost cause, don't you? #speaker:Anton #portrait:Character_Anton_Neutral
    Not at all. #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
    I just want to make sure you're safe.
    By keeping me locked up in the house? #speaker:Anton #portrait:Character_Anton_Neutral
    By making sure we know where you are. #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
    ... #speaker:Anton #portrait:Character_Anton_Neutral
    I'm going now. #speaker:Anton #portrait:Character_Anton_Sad
    ... #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
    Okay. 
    Take care, Mahal.
-
Hm. #speaker:Anton #portrait:Character_Anton_Neutral #effect:none
~ p2WifeDone = true
-> DONE

=== phase2DishSoapReceived ===
The door looks good. #speaker:Anton #portrait:Character_Anton_Neutral
Aw! #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
Thank you, Mahal.
-> DONE

=== phase3start ===
Hello, Mahal. #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
... #speaker:Anton #portrait:Character_Anton_Neutral
Hm.
-> DONE

=== phase3check ===
{ p3HaveBox and p3WifeDone:
    -> phase3BoxReceived
  - else:
    -> phase3GetBox
}

=== phase3GetBox ===
Rosa? #speaker:Anton #portrait:Character_Anton_Neutral
Hm? #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
Oh! #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
Yes, mahal?
We need one more box for them. #speaker:Anton #portrait:Character_Anton_Neutral
Do you have any?
We should have more... #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
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
We can handle ourselves!
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
Don't you need to give that box to someone? 
Oh. #speaker:Anton #portrait:Character_Anton_Neutral
Right.
I'll go do that.
Thank you.
You're welcome, Mahal. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
~ p3WifeDone = true
-> DONE

=== phase3BoxReceived ===
Did you give the box yet? #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
Box? #speaker:Anton #portrait:Character_Anton_Neutral
Oh.
Not yet.
Go do that, Mahal. #speaker:Rosalyn #portrait:Character_Rosalyn_Neutral
I'll be okay here. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
-> DONE

=== phase4 ===
It's almost time... #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
Rosa? #speaker:Anton #portrait:Character_Anton_Neutral
I'll be okay... #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
... #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase5 ===
Mahal? #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
Rosa? #speaker:Anton #portrait:Character_Anton_Neutral
They sent a letter.
Who?
Benji and Liza. #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
Oh. #speaker:Anton #portrait:Character_Anton_Neutral
What does it say?
A goodbye and well wishes... #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
They said they'll try to visit every month. 
{ sonPieces > 8 and dilPieces > 8:
    -> phase5GoodEnd
- else:
    -> phase5BadEnd
}
= phase5GoodEnd
Mm. #speaker:Anton #portrait:Character_Anton_Neutral
They better bring our grandchild.
I'm sure they will, Mahal. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
When the time comes.
...
It's just the two of us again. #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
...
Yes, it is. #speaker:Anton #portrait:Character_Anton_Neutral
It seems like the first time in a long time.
The house feels... #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
Bigger...
... #speaker:Anton #portrait:Character_Anton_Neutral
We'll find a way to fill the space again. 
Together.
... #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
Together.
... #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
So... #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
What would you like for dinner?
...
~ gamePhase = 6
-> DONE

= phase5BadEnd
Tell them not to bother. #speaker:Anton #portrait:Character_Anton_Neutral #effect:tense
If we're just a nuisance to them anyway.
Anton. #speaker:Ros?ly? #portrait:Character_Rosalyn_Angry
What? #speaker:Anton #portrait:Character_Anton_Angry
If it's such a chore being around us, then they don't need to be around.
\(sigh) #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
Anton... 
You know this is why they left. right? 
You keep fighting us.
You make it hard for us to take care of you.
I don't need to be taken care of! #speaker:Anton #portrait:Character_Anton_Angry
Mahal. #speaker:Rosalyn #portrait:Character_Rosalyn_Sad
It's okay. #speaker:Rosalyn #portrait:Character_Rosalyn_Happy
We need some help and that's okay.
... #speaker:Anton #portrait:Character_Anton_Neutral
Hmp.
We don't need any help. 
We can handle ourselves just fine.
... #speaker:R?salyn #portrait:Character_Rosalyn_Sad
I'm... 
I'm not so sure about that anymore...
...
What?
Mahal. #speaker:Rosal?n #portrait:Character_Rosalyn_Sad
I'm sorry, but...
Just thinking practically.
We're getting old.
And this house is too big for just both of us.
A helping hand would be nice.
And they could help me take care of you too...
... #speaker:Anton #portrait:Character_Anton_Neutral
I don't want to fight over this, Mahal. #speaker:Rosaly? #portrait:Character_Rosalyn_Sad
I...
I think it's for our own good too.
... #speaker:Anton #portrait:Character_Anton_Neutral
Fine.
Thank you, Mahal. #speaker:Ros?l?n #portrait:Character_Rosalyn_Neutral
And... #speaker:R?sal?n #portrait:Character_Rosalyn_Sad
I truly am sorry.
... #speaker:Anton #portrait:Character_Anton_Neutral
Hm.
... #speaker:Rosal?? #portrait:Character_Rosalyn_Sad
... #speaker:Anton #portrait:Character_Anton_Sad
~ gamePhase = 6
-> DONE