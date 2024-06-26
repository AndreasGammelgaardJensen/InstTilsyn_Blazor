﻿using DataAccess.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class DbContextFactory : IDesignTimeDbContextFactory<DataContext>
{
             

    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        var connectionstring = Environment.GetEnvironmentVariable("SQLConnectionString");
        optionsBuilder.UseSqlServer(connectionstring);
        optionsBuilder.EnableSensitiveDataLogging(false);
        

        return new DataContext(optionsBuilder.Options);
    }
    //Generate CreateContext Method where i can add logging filter to the optionsBuilder    
}