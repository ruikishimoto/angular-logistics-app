﻿namespace Logistics.Application.Tenant.Models;

public record MonthlyGrossDto(int Year, int Month)
{
    public double Income { get; set; }
    public double Distance { get; set; }
}