﻿using System.ComponentModel.DataAnnotations;

namespace Logistics.DriverApp.Models;

public class AccountEditForm : ObservableValidator
{
    private string? _userName;
    private string? _email;
    private string? _firstName;
    private string? _lastName;
    private string? _phoneNumber;

    [Required]
    public string? UserName
    {
        get => _userName; 
        set => SetProperty(ref _userName, value);
    }

    [Required, EmailAddress]
    public string? Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    [Required, StringLength(64)]
    public string? FirstName
    {
        get => _firstName; 
        set => SetProperty(ref _firstName, value);
    }

    [Required, StringLength(64)]
    public string? LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }

    [Phone]
    public string? PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);
    }

    public void Validate()
    {
        ValidateAllProperties();
    }
}
