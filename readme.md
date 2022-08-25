
The goal: removing the need for water tanks.

Playing around with dnspy I found the code that waters tiles is in sprinklerTiles.waterTiles()

This function first makes sure that the sprinkler is not actually a silo (silos are also "sprinklers" in the code) then checks to see if the sprinkler is in range of a water tank.  This is done by setting a boolean called **flag** to false, then marking it true if one of the water tanks on the map is in range.

We're going to instead set it to true from from the very start.  This is done by editing the IL Code with a Harmony Transiler patch.  




We will turn this:

> public void waterTiles(int xPos, int yPos, List<int[]> waterTanks)
> {
> 	if (!this.isSilo)
> 	{
> 		bool flag = false;
> 		for (int i = 0; i < waterTanks.Count; i++)
> 		{
> 			[...]
 			
Into this: 

> public void waterTiles(int xPos, int yPos, List<int[]> waterTanks)
> {
> 	if (!this.isSilo)
> 	{
> 		bool flag = true;
> 		for (int i = 0; i < waterTanks.Count; i++)
> 		{
> 			[...]
			
by changing this:
			
> /* (12,3)-(12,20) main.cs */
> /* 0x00000000 02           */ IL_0000: ldarg.0
> /* 0x00000001 7B77170004   */ IL_0001: ldfld     bool SprinklerTile::isSilo
> /* 0x00000006 3A2E030000   */ IL_0006: brtrue    IL_0339
> 
> /* (15,9)-(15,18) main.cs */
> /* 0x0000000B 16           */ IL_000B: ldc.i4.0
> /* 0x0000000C 0A           */ IL_000C: stloc.0
> /* (hidden)-(hidden) main.cs */
> /* 0x0000000D 38B8000000   */ IL_000D: br        IL_00CA
> // loop start (head: IL_00CA)


into this:

> /* (12,3)-(12,20) main.cs */
> /* 0x00000000 02           */ IL_0000: ldarg.0
> /* 0x00000001 7B77170004   */ IL_0001: ldfld     bool SprinklerTile::isSilo
> /* 0x00000006 3A2E030000   */ IL_0006: brtrue    IL_0339
> 
> /* (15,9)-(15,18) main.cs */
> /* 0x0000000B 16           */ IL_000B: ldc.i4.1
> /* 0x0000000C 0A           */ IL_000C: stloc.0
> /* (hidden)-(hidden) main.cs */
> /* 0x0000000D 38B8000000   */ IL_000D: br        IL_00CA
> // loop start (head: IL_00CA)	


... a lot of nothing working later

The IL Code that dnSpy shows and the ILCode that the Harmony patcher actually gets are very different.

dnSpy:

IL_0000: ldarg.0
IL_0001: ldfld     bool SprinklerTile::isSilo
IL_0006: brtrue    IL_0339
IL_000B: ldc.i4.0  
[...]


dumping the code from within the harmony Patch function with foreach(CodeInstruction x in instructions):

ldarg.0 NULL
ldfld bool SprinklerTile::isSilo
brfalse Label1
[...127 lines later...]
ldc.i4.0 NULL [Label1]




