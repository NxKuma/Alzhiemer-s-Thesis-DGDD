INCLUDE globals.ink

{ gamePhase:
- 0: //tutorial
- 1: -> phase1
- 1.1: -> phase1check
- 2: -> phase2start
- 2.1: -> phase2check
- 3: -> phase3start
- 3.1: -> phase3check
- 4: -> phase4start
- 4.1: -> phase4check
}

=== phase1 ===
Hey, kid. #speaker:Anton #portrait:Character_Anton_Neutral
Hey, Pa. #speaker:B?nj? #portrait:Character_Benji_Neutral
-> DONE

=== phase1check ===
{p1SonDone and p1HaveRoller:
    -> phase1RollerReceived
- else:
    -> phase1GetRoller
}

=== phase1GetRoller ===
Hey, kid. #speaker:Anton #portrait:Character_Anton_Neutral
Benji, Pa. #speaker:Benji #portrait:Character_Benji_Sad
... #speaker:Anton #portrait:Character_Anton_Neutral
Sorry. #speaker:Anton #portrait:Character_Anton_Neutral
\(sigh) #speaker:Benji #portrait:Character_Benji_Neutral
It's fine.
Don't worry about it.
Did you need something?
I did, yeah. #speaker:Anton #portrait:Character_Anton_Neutral
I was looking for some paints. 
Oh. #speaker:Benj? #portrait:Character_Benji_Neutral
I'm not sure where Ma put it. 
Sorry.
That's fine. #speaker:Anton #portrait:Character_Anton_Neutral
I'll keep looking. 
Do you have any paint brushes or something?
She asked for some too.
Uhh... #speaker:Be??i #portrait:Character_Benji_Neutral
I have a paint roller, is that okay?
That should work. #speaker:Anton #portrait:Character_Anton_Neutral
Alright. #speaker:Ben?i #portrait:Character_Benji_Neutral
Just get it back to me after you're done with it, please.
I'm planning to bring it with us to the new house for our own redecorating. 
Actually...
Pa, can you help us later with moving everything?
+ ... #speaker:Anton #portrait:Character_Anton_Sad 
    Do you really have to go? #effect:vignette
    We've talked about this, Pa. #speaker:B?nji #portrait:Character_Benji_Sad
    It'll be easier for me and Liza.
    And with a child on the way,
    We want the best for him too.
    ... #speaker:Anton #portrait:Character_Anton_Sad
    I understand.
    It's just...
+ Sure. #speaker:Anton #portrait:Character_Anton_Neutral
    What do you need help with?
    I think it's just gonna be a lot of boxes. #speaker:B??ji #portrait:Character_Benji_Neutral
    It'd go faster with more people.
    So you'd be a big help. #speaker:B???i #portrait:Character_Benji_Happy
    Alright. #speaker:Anton #portrait:Character_Anton_Neutral
    ... #speaker:Anton #portrait:Character_Anton_Sad
-
We'll miss you. #speaker:Anton #portrait:Character_Anton_Sad #effect:vignette
I'll miss you too, Pa. #speaker:Benji #portrait:Character_Benji_Sad
We both will.
But, we'll be sure to visit. #speaker:Benji #portrait:Character_Benji_Happy
You better. #speaker:Anton #portrait:Character_Anton_Neutral
And bring little Anthony with you!
We haven't decided on a name yet, Pa! #speaker:Benji #portrait:Character_Anton_Neutral
And sorry to say, #speaker:Benj? #portrait:Character_Benji_Sad
We're probably not naming him after you.
Well, why not? #speaker:Anton #portrait:Character_Anton_Angry
Anthony is a good name! #speaker:Anton #portrait:Character_Anton_Neutral
Strong. 
Reliable.
Sure, Pa. #speaker:Ben?? #portrait:Character_Benji_Happy
But that's your name.
We want him to have his own.
Hm. #speaker:Anton #portrait:Character_Anton_Neutral
I guess that's good too.
Thanks, Pa. #speaker:Benj? #portrait:Character_Benji_Happy #effect:none
~ p1SonDone = true
-> DONE

=== phase1RollerReceived ===
Hey, Pa. #speaker:Benji #portrait:Character_Benji_Neutral #effect:none
Need anything else?
Not right now. #speaker:Anton #portrait:Character_Anton_Neutral
Thanks.
-> DONE

=== phase2start ===
... #speaker:Anton #portrait:Character_Anton_Neutral
Hey, uh-
Hm? #speaker:B???? #portrait:Character_Benji_Neutral
Hey, kid. #speaker:Anton #portrait:Character_Anton_Neutral
\(sigh) #speaker:Benji #portrait:Character_Benji_Sad
Benji.
... #speaker:Anton #portrait:Character_Anton_Sad
Yeah. #speaker:Anton #portrait:Character_Anton_Neutral
Benji.
I knew-
Know.
That.
I know that.
Of course I know that.
Of course. #speaker:Benji #portrait:Character_Benji_Sad
Actually... #speaker:Anton #portrait:Character_Anton_Neutral
Aren't you supposed to be at school right now. 
What? #speaker:Ben?i #portrait:Character_Benji_Neutral #effect:tense
Pa, I graduated 4 years ago.
I'm a grown, working adult.
Sales, remember?
Oh. #speaker:Anton #portrait:Character_Anton_Sad
Right.
Sorry.
It's fine. #speaker:Be??i #portrait:Character_Anton_Sad
... #speaker:Anton #portrait:Character_Anton_Sad #effect:none
Do you need help with the dishes?
I'm good for now. #speaker:B?n?? #portrait:Character_Benji_Sad
...
Oh, actually... #speaker:Be??i #portrait:Character_Benji_Neutral
We're almost out of dish soap.
Can you get some?
And some trash bags too.
It's filled up.
+ Sure. #speaker:Anton #portrait:Character_Anton_Neutral
    Where'd we put them?
    Check by the front door, maybe. #speaker:Be?j? #portrait:Character_Benji_Neutral
    Not sure if we packed away after groceries.
    Okay. #speaker:Anton #portrait:Character_Anton_Neutral
+ [No.]
    So bossy. #speaker:Anton #portrait:Character_Anton_Neutral
    Why are you making this old man do so much? #speaker:Anton #portrait:Character_Anton_Neutral #effect:none
    It's just a few thing, Pa! #speaker:B??ji #portrait:Character_Benji_Neutral
    You can handle yourself.
    And I still have all the oily pots to wrestle with.
    So, please?
    Can you get the stuff?
    Hmp. #speaker:Anton #portrait:Character_Anton_Neutral
    Fine.
-
Thank you, Pa. #speaker:B?nj? #portrait:Character_Benji_Happy
~ gamePhase = 2.1
-> DONE

=== phase2check ===
Did you find the stuff yet? #speaker:B???i #portrait:Character_Benji_Neutral #effect:none
Uh... #speaker:Anton #portrait:Character_Anton_Neutral
\(sigh)  #speaker:B???i #portrait:Character_Benji_Neutral
The dish soap and the trashbags.
Right. #speaker:Anton #portrait:Character_Anton_Neutral
Uh.
+ [Not yet.] #speaker:Anton #portrait:Character_Anton_Neutral
    No, not yet.
    Okay. #speaker:B??j? #portrait:Character_Benji_Sad
    Let me know when you do.
+ [Yes.] #speaker:Anton #portrait:Character_Anton_Neutral
    {p2HaveSoap and p2HaveTrash and p2WifeDone and p2DILDone:
        -> phase2TrashBagsReceived
    - else:
        ...? #speaker:B?n?? #portrait:Character_Benji_Neutral
        Pa... #speaker:B?n?? #portrait:Character_Benji_Sad #effect:tense
        ... #speaker:Anton #portrait:Character_Anton_Neutral
        Sorry.
        It's fine. #speaker:B??ji #portrait:Character_Benji_Sad
        Let me know when you find them.
    }
-
-> DONE

=== phase2TrashBagsReceived ===
#test here if enough pieces
{ sonPieces > 6:
    -> recognized
    - else:
    -> notRecognized
        
}
= recognized
Here. #speaker:Anton #portrait:Character_Anton_Neutral
Thanks, Pa. #speaker:Benj? #portrait:Character_Benji_Happy
Are you about done with the dishes? #speaker:Anton #portrait:Character_Anton_Neutral
Yeah, I am. #speaker:Benji #portrait:Character_Benji_Neutral
I'll finish up here.
Go sit on the couch and watch TV or something.
Hm. #speaker:Anton #portrait:Character_Anton_Neutral
~ gamePhase = 3
-> DONE

= notRecognized
What are you doing in my house?! #speaker:Anton #portrait:Character_Anton_Angry #effect:tense
What? #speaker:????? #portrait:Character_Benji_Neutral
Give me that! #speaker:Anton #portrait:Character_Anton_Angry
Wha- #speaker:B???? #portrait:Character_Benji_Angry #effect:shake
Pa! Hey! 
Let go! #speaker:Anton #portrait:Character_Anton_Angry #effect:shake
I'm already in a bad mood.
So don't make me fight you!
Okay! #speaker:B???i #portrait:Character_Benji_Angry
Okay! I'm letting go!
What's going on in here?! #speaker:Rosalyn #portrait:Character_Rosalyn_Angry #effect:shake
I don't know! #speaker:B??j? #portrait:Character_Benji_Angry
He just stormed in, already angry!
Mahal! #speaker:Rosalyn #portrait:Character_Rosalyn_Sad #effect:none
It's me! #effect:vignette
It's me, Rosa.
Your wife.
It's okay.
You're okay.
You're safe, Mahal. 
You're with family. #effect:none
This is your son, Benji, remember?
We live together.
All four of us, together.
You, me, Benji, and Liza.
Our family.
... #speaker:Anton #portrait:Character_Anton_Sad
Our...
Family...
// black out here
~ gamePhase = 3
-> DONE

=== phase3start ===
... #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase3check ===
// need to have talked to wife and gotten the box
{ p3WifeDone and p3HaveBox: 
    -> phase3BoxReceived
  - else:
    -> phase3GetBox 
}

=== phase3GetBox ===
Did you find the box, Pa? #speaker:B???i #portrait:Character_Benji_Neutral
Not yet. #speaker:Anton #portrait:Character_Anton_Neutral
Let me know when you do. #speaker:Be??i #portrait:Character_Benji_Neutral
-> DONE

=== phase3BoxReceived ===
I have the box. #speaker:Anton #portrait:Character_Anton_Neutral
Thanks, Pa. #speaker:??nji #portrait:Character_Benji_Happy
... #speaker:B??ji #portrait:Character_Benji_Neutral
Say...
Can you help Liza with the packing?
... #speaker:Anton #portrait:Character_Anton_Neutral
What. #effect:tense
I know you two are... #speaker:B???? #portrait:Character_Benji_Neutral
At odds...
But it doesn't feel right to leave things as they are.
Especially since we're leaving soon.
It'll be a while before we see each other again.
No. #speaker:Anton #portrait:Character_Anton_Neutral
Please, Pa? #speaker:Be??? #portrait:Character_Benji_Neutral
I'll talk to her too.
Just-
Talk to each other?
Please.
+ [No.]
    I'd rather not. #speaker:Anton #portrait:Character_Anton_Neutral
    Pa... #speaker:B???? #portrait:Character_Benji_Sad
    \(sigh)
    Why do you have to be so stubborn? #speaker:????? #portrait:Character_Benji_Sad
    Fine. #speaker:????? #portrait:Character_Benji_Neutral
    You don't have to talk to her.
    Can you at least help with the boxes?
    ...  #speaker:Anton #portrait:Character_Anton_Neutral
    Fine.
    Thank you. #speaker:????? #portrait:Character_Benji_Happy
+ [Fine.]
    ... #speaker:Anton #portrait:Character_Anton_Neutral
    Fine.
    Thank you. #speaker:Ben?? #portrait:Character_Benji_Happy
-
~ gamePhase = 4
-> DONE

=== phase4start ===
I can handle the things here. #speaker:B???? #portrait:Character_Benji_Neutral
Go find Liza, Pa.
... #speaker:Anton #portrait:Character_Anton_Neutral
-> DONE

=== phase4check ===
{ p4HaveGame:
    -> phase4PuzzleReceived
  - else:
    -> phase4GetPuzzle
}

=== phase4GetPuzzle ===
Hm? #speaker:Be??? #portrait:Character_Benji_Neutral
Uh... #speaker:Anton #portrait:Character_Anton_Neutral
I'm forgetting something.
What is it? #speaker:B?nj? #portrait:Character_Benji_Neutral
It's uh... #speaker:Anton #portrait:Character_Anton_Neutral #effect:tense
It's fine. #effect:none
I'll remember.
... #speaker:B???? #portrait:Character_Benji_Sad
-> DONE

=== phase4PuzzleReceived ===
{ sonPieces > 8:
    -> phase4RememberBenji
- else:
    -> phase4ForgotBenji
}
= phase4RememberBenji
I found the puzzle. #speaker:Anton #portrait:Character_Anton_Neutral
Oh! #speaker:Benji #portrait:Character_Benji_Happy
That's great, Pa!
Thanks for finding it. #speaker:Benji #portrait:Character_Benji_Happy
You should open it to check if it still has all its pieces. #speaker:Anton #portrait:Character_Anton_Neutral
Right. #speaker:Benji #portrait:Character_Benji_Happy
... #speaker:Benji #portrait:Character_Benji_Neutral
Wanna piece it together, Pa?
Just like old times. #effect:vignette
... #speaker:Anton #portrait:Character_Anton_Neutral 
Just like old times.
...
When did we last put this together? #speaker:Anton #portrait:Character_Anton_Neutral 
It's been years. #speaker:Ben?i #portrait:Character_Benji_Neutral 
I'm surprised it's still in good condition.
We kept it safe. #speaker:Anton #portrait:Character_Anton_Neutral 
It's...
A treasure.
For me.
... #speaker:Benji #portrait:Character_Benji_Happy
Thanks, Pa.
Hm. #speaker:Anton #portrait:Character_Anton_Neutral 
...
Seems like it's complete.
And all the pieces are here.
That's good to hear. #speaker:Benji #portrait:Character_Benji_Neutral #effect:none
-> phase4PuzzleEnd

= phase4ForgotBenji
I've missed this puzzle. #speaker:Anton #portrait:Character_Anton_Neutral
Oh!
You found it!
I wonder if the pieces are still complete. #speaker:Anton #portrait:Character_Anton_Neutral
...
Want to help me out, kid? #effect:vignette
... #speaker:????? #portrait:Character_Benji_Sad
Of course. 
... #speaker:Anton #portrait:Character_Anton_Neutral
... #speaker:????? #portrait:Character_Benji_Sad
This feels... #speaker:Anton #portrait:Character_Anton_Neutral
Nostalgic. #speaker:Anton #portrait:Character_Anton_Happy
You remind me of my son.
We used to piece puzzles just like this when he was a kid.
...
I miss those days.
... #speaker:????? #portrait:Character_Benji_Sad
I miss you too, Pa.
Hm? #speaker:Anton #portrait:Character_Anton_Neutral
Hah! #speaker:Anton #portrait:Character_Anton_Happy
Very similar indeed.
... #speaker:????? #portrait:Character_Benji_Sad
Hm. #speaker:Anton #portrait:Character_Anton_Sad #effect:none
It's missing a few pieces.
Sorry about that, kid. #speaker:Anton #portrait:Character_Anton_Neutral 
I guess I must have lost them at some point.
... #speaker:Anton #portrait:Character_Benji_Sad
It's alright, Pa.
Thanks anyway.
...? #speaker:Anton #portrait:Character_Anton_Neutral 
-> phase4PuzzleEnd

= phase4PuzzleEnd
Oh, you found it. #speaker:L??? #portrait:Character_Liza_Neutral #effect:none
If you're about done, I'll put it in the luggage, now.
// TAKEN FROM LIZA'S FILE
{ dilPieces > 8:
    -> phase4RememberLiza
- else:
    -> phase4ForgotLiza
}
= phase4RememberLiza
Mm. #speaker:Anton #portrait:Character_Anton_Neutral
... #speaker:Liza #portrait:Character_Liza_Neutral
Thanks. #effect:vignette
For helping out with the move. 
... #speaker:Anton #portrait:Character_Anton_Neutral
Mm.
... #speaker:Benj? #portrait:Character_Benji_Neutral
Well... #effect:none
That's a start, I suppose.
Really, though, Pa. #speaker:Benji #portrait:Character_Benji_Happy
Thanks. #speaker:Benji #portrait:Character_Benji_Happy
Mm. #speaker:Anton #portrait:Character_Anton_Neutral
You're welcome.
... #speaker:Benji #portrait:Character_Benji_Happy
The truck's arriving in an hour. #speaker:Benji #portrait:Character_Benji_Neutral
We'll do one last check of our things.
Then...
We'll be going, I suppose. #speaker:Benji #portrait:Character_Benji_Sad 
... 
... #speaker:Anton #portrait:Character_Anton_Neutral
We'll miss you.
... #speaker:Benj? #portrait:Character_Benji_Happy
We'll miss you too, Pa. 
You guys take care here, okay? #speaker:Benji #portrait:Character_Benji_Neutral
We will. #speaker:Anton #portrait:Character_Anton_Neutral #effect:none
You take care too.
See you around, Pa.
//blackout
~gamePhase = 5
-> DONE

= phase4ForgotLiza
What are you- #speaker:Anton #portrait:Character_Anton_Neutral #effect:tense
Hey! #speaker:Anton #portrait:Character_Anton_Angry
This again... #speaker:??? #portrait:Character_Liza_Neutral
Don't touch that! #speaker:Anton #portrait:Character_Anton_Angry #effect:shake
What now?! #speaker:??? #portrait:Character_Liza_Angry
Let go of that puzzle box! #speaker:Anton #portrait:Character_Anton_Angry #effect:shake
+ [Take it back.]
+ [Take it back.]
+ [Take it back.]
-
What are you doing?! #speaker:??? #portrait:Character_Liza_Angry
Let go! #speaker:Anton #portrait:Character_Anton_Angry
This is my son's treasure!
You are not taking it from me! #effect:shake
From us! #effect:shake
+ [Take. It. Back.]
+ [Take. It. Back.]
+ [Take. It. Back.]
-
This is the last thing for the move! #speaker:??? #portrait:Character_Liza_Angry
We're literally done after this!
Stop being so stubborn!
And let. #effect:shake
Us. #effect:shake
Go! #effect:shake
+ [Protect your family.]
-
AH! #speaker:??? #portrait:Character_Liza_Angry #effect:shake
What's going on- #speaker:??? #portrait:Character_Benji_Neutral
Liza?! #speaker:??? #portrait:Character_Benji_Angry #effect:tense
Pa, what did you do?! 
...What? #speaker:Anton #portrait:Character_Anton_Sad
What happened? #speaker:Rosaly? #portrait:Character_Rosalyn_Neutral
Oh my god. #speaker:Rosal?? #portrait:Character_Rosalyn_Sad
Anton, what did you do?
I- #speaker:Anton #portrait:Character_Anton_Sad
I didn't-
She was-
Oh, thank god. #speaker:??? #portrait:Character_Benji_Sad
She's okay.
...
Maybe it's a good thing we're moving after all.
... #speaker:Rosa??? #portrait:Character_Rosalyn_Sad
... #speaker:Anton #portrait:Character_Anton_Sad
...You should get a caregiver, Ma. #speaker:??? #portrait:Character_Benji_Sad
I don't want this to happen to you too. #speaker:??? #portrait:Character_Benji_Neutral
I would never- #speaker:Anton #portrait:Character_Anton_Angry
You just did, Pa. #speaker:B???? #portrait:Character_Benji_Angry
You just did. #speaker:B???? #portrait:Character_Benji_Sad
...
The truck's coming in an hour. #speaker:B???? #portrait:Character_Benji_Neutral
Stay away from Liza for the time being.
Please.
... #speaker:Anton #portrait:Character_Anton_Sad
I'm so sorry.
~ gamePhase = 5
-> DONE