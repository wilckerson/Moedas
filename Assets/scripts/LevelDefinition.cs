using System.Collections;
using System.Collections.Generic;
using System;

public class LevelDefinition
{
	public Guid Id { get; set; }
	public int Start1Time { get; set; }

	public int Start2Time { get; set; }

	public int Start3Time { get; set; }
}

public static class LevelDefinitions
{

	public static List<LevelDefinition> Levels = new List<LevelDefinition> (){
		new LevelDefinition(){
			Id = new Guid("315841c3-396a-4f54-8a94-3e6430c350a0"),
			Start1Time = 60,
			Start2Time = 90,
			Start3Time = 120
		},
		new LevelDefinition(){
			Id = new Guid("eed445b4-d281-4fa5-a318-29c0086ce9eb"),
			Start1Time = 80,
			Start2Time = 100,
			Start3Time = 150
		},
	};
}