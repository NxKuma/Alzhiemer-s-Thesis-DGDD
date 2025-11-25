INCLUDE globals.ink

{ gamePhase:
- 0: //tutorial
- 0.1: //
- 1: -> phase1
- 1.1: -> phase1check
- 2: //phase2
- 3: //phase3
}

=== phase1 ===
Hey, kid. #speaker:Anton #portrait:anton
Hey, pa. #speaker:B?nj? #portrait:benji
-> DONE

=== phase1check ===
{p1q1done:
    -> phase1RollerReceived
- else:
    -> phase1GetRoller
}

=== phase1GetRoller ===
Hey, kid. #speaker:Anton #portrait:anton
Benji, Pa. #speaker:Benji #portrait:benji
... #speaker:Anton #portrait:anton
Sorry.
It's fine. #speaker:Benji #portrait:benji
Don't worry about it.
Did you need something?
Yeah, I was looking for some paints. #speaker:Anton #portrait:anton
Oh. #speaker:Benj? #portrait:benji
I'm not sure where Ma put it. 
Sorry.
That's okay. I'll keep looking. #speaker:Anton #portrait:anton
Do you have any paint brushes or something?
She asked for some too.
Uhh... #speaker:Be??i #portrait:benji
I have a paint roller, is that okay?
That works, thanks. #speaker:Anton #portrait:anton
I was gonna bring it with us to the new house for our own redecorating. #speaker:Ben?i #portrait:benji
I'll just get it from her later when we finish packing.
Pa, can you help us later with moving everything?
+ ... #speaker:Anton #portrait:anton
    Do you really have to go?
    We've talked about this, Pa. #speaker:B?nji #portrait:benji
    It'll be easier for me and Liza.
    And with a child on the way,
    We want the best for him too.
    ... #speaker:Anton #portrait:anton
    I understand.
    It's just...
+ Sure. #speaker:Anton #portrait:anton
    What do you need help with?
    I just think it's gonna be a lot of things. #speaker:B??ji #portrait:benji
    It'd go faster with more people. 
    Alright. #speaker:Anton #portrait:anton
    ...
-
We'll miss you. #speaker:Anton #portrait:anton
I'll miss you too, Pa. #speaker:Benji #portrait:benji
We both will.
We'll be sure to visit. 
You better. #speaker:Anton #portrait:anton
And bring little Anthony with you!
We haven't decided on a name yet, Pa! #speaker:Benji #portrait:benji
And sorry to say,
We're probably not naming him after you.
Well, why not? #speaker:Anton #portrait:anton
Anthony is a good name! 
Strong. 
Reliable.
Sure, Pa. #speaker:Ben?? #portrait:benji
But that's your name.
We want him to have his own.
Hm. #speaker:Anton #portrait:anton
I guess I can be okay with that.
Thanks, Pa. #speaker:Benj? #portrait:benji
~ p1q1done = true
-> DONE

=== phase1RollerReceived ===
Hey, Pa. #speaker:Benji #portrait:benji
Need anything else?
Not right now. #speaker:Anton #portrait:anton
Thanks.
-> DONE