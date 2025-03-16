using Microsoft.Extensions.Caching.Distributed;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System.Text.Json;

public class AddressRL : IAddressRL
{
    private readonly AddressBookContext _dbContext;
    private readonly IDistributedCache _cache;

    public AddressRL(AddressBookContext dbContext, IDistributedCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public List<AddressEntryEntity> GetAllAddress()
    {
        string cacheKey = "all_contacts";
        var cachedData = _cache.GetString(cacheKey);

        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<List<AddressEntryEntity>>(cachedData);
        }

        var result = _dbContext.AddressBooks.ToList();
        if (result != null)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            _cache.SetString(cacheKey, JsonSerializer.Serialize(result), cacheOptions);
        }
        return result;
    }

    public AddressEntryEntity GetAddressByID(int id)
    {
        string cacheKey = $"contact_{id}";
        var cachedData = _cache.GetString(cacheKey);

        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<AddressEntryEntity>(cachedData);
        }

        var result = _dbContext.AddressBooks.FirstOrDefault(x => x.id == id);
        if (result != null)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            _cache.SetString(cacheKey, JsonSerializer.Serialize(result), cacheOptions);
        }
        return result;
    }

    public AddContactModel AddContact(AddContactModel newContact)
    {
        var result = _dbContext.AddressBooks.FirstOrDefault(x => x.email == newContact.email);
        if (result == null)
        {
            var addressEntry = new AddressEntryEntity()
            {
                name = newContact.name,
                address = newContact.address,
                phone = newContact.phone,
                email = newContact.email
            };

            _dbContext.AddressBooks.Add(addressEntry);
            _dbContext.SaveChanges();

            _cache.Remove("all_contacts"); // Invalidate cache

            return newContact;
        }
        return null;
    }

    public UpdateContactModel UpdateContact(int id, UpdateContactModel updateContact)
    {
        var result = _dbContext.AddressBooks.FirstOrDefault(x => x.id == id);
        if (result != null)
        {
            result.name = updateContact.name;
            result.address = updateContact.address;
            result.phone = updateContact.phone;
            result.email = updateContact.email;

            _dbContext.SaveChanges();

            _cache.Remove("all_contacts");
            _cache.Remove($"contact_{id}");

            return updateContact;
        }
        return null;
    }

    public bool DeleteContact(int id)
    {
        var result = _dbContext.AddressBooks.FirstOrDefault(x => x.id == id);
        if (result != null)
        {
            _dbContext.AddressBooks.Remove(result);
            _dbContext.SaveChanges();

            _cache.Remove("all_contacts");
            _cache.Remove($"contact_{id}");

            return true;
        }
        return false;
    }
}
