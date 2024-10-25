﻿namespace Passbook.Generator.Tags;

/// <summary>
/// The unique abbreviation of the homee team’s name. Use this key only for a sports event ticket.
/// </summary>
public class HomeTeamAbbreviation(string value) : SemanticTagBaseValue("homeTeamAbbreviation", value)
{
}
