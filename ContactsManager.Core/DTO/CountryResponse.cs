﻿using System;
using System.Collections.Generic;
using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Used as a return type for most of the CountriesService Method. 
    /// </summary>
    public class CountryResponse
    {
        public Guid Id { get; set; }
        public string? CountryName { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if(obj.GetType() != typeof(CountryResponse)) return false;
            CountryResponse other = (CountryResponse)obj;
            return CountryName == other.CountryName && Id == other.Id;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
    public static class CountryExtentions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryName = country.CountryName,
                Id = country.CountryId
            };
        }
    }
}
