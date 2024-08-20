each dialogue is represented as JSON, in a single file. 
the root of the json model in the file is an array, with each element being an object representing a single "statement" of the NPC who is going to speak this dialogue. a "statement" is the block of text that would appear in a single UI box of dialogue that represents what the NPC is saying. 
each statement can stand alone, have some player dialogue responses with it so the user can choose how to respond, or have an in-game event or action be triggered after it. 

the json should be structured like this:
statement has the following properties, with the appropriate types:
    id: int (required)
    text: string (required): the text of the statement to display in the dialogue UI box
    responses: array (optional): a list of response objects that the player can choose to progress the dialogue
    action: int (optional): the id of the action/event to trigger
    next: int (optional): the id of the next statement in the dialogue, if there are no responses. -1 for exit dialogue
    speed: int : speed at which text appears on GUI
    fontSize: int (optional)
    emotion: int (optional): the emotion of the NPC during this statement

while some of these are optional, there are some constraints of what properties need to exist. 
if have responses, don't have next, because the next statement comes after a response, which is defined in that response. 
if have next, don't have responses. dialogue will just go to the next statement. 
if have neither responses nor next, exit dialogue after this statement. 
if have action, can have any of the prior described combinations of responses and next. in any case, the action will be triggered after the user progresses to next statement or exits dialogue. 

response object has the following properties:
    text: string (required): the text of what the player says to respond to the statement
    next: int (optional): the id of the statement that should come after this response. -1 for exit dialogue

if response has no next, then dialogue will exit after the response. 

see some example dialogue json files in assets/dialogue/