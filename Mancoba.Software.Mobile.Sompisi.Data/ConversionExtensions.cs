using System.Collections.Generic;
using Mancoba.Sompisi.Data.LocalDb.Entities;
using Mancoba.Sompisi.Data.Models;

namespace Mancoba.Sompisi.Data
{
    internal static class ConversionExtensions
    {
        #region SystemUser

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        internal static ModelSystemUser ToModel(this EntitySystemUser entity)
		{
			var model = new ModelSystemUser();
			model.Id = entity.Id;		
			model.Username = entity.Username;
			model.FirstName = entity.FirstName;
			model.LastName = entity.LastName;
			
			model.Password = entity.Password;

            model.EmailAddress = entity.EmailAddress;
            model.PhoneNumber = entity.PhoneNumber;
			model.MobileNumber = entity.MobileNumber;

			model.StreetId = entity.StreetId;
			model.House = entity.House;
			model.Street = entity.Street;
			model.Suburb = entity.Suburb;
			model.Town = entity.Town;
			model.Province = entity.Province;
			model.Country = entity.Country;

			return model;
		}

        /// <summary>
        /// To the entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        internal static EntitySystemUser ToEntity(this ModelSystemUser model)
        {
            var entity = new EntitySystemUser();
            entity.Id = model.Id;
            entity.Username = model.Username;
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;           
            entity.Password = model.Password;

            entity.EmailAddress = model.EmailAddress;
            entity.PhoneNumber = model.PhoneNumber;
            entity.MobileNumber = model.MobileNumber;

			entity.StreetId = model.StreetId;
			entity.House = model.House;
			entity.Street = model.Street;
			entity.Suburb = model.Suburb;
			entity.Town = model.Town;
			entity.Province = model.Province;
			entity.Country = model.Country;

			return entity;
        }

        #endregion

        #region Provider

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        internal static ModelProvider ToModel(this EntityProvider entity)
        {
            var model = new ModelProvider();
			model.Id = entity.Id;
			model.Name = entity.Name;
			model.ContactPerson = entity.ContactPerson;
			model.EmailAddress = entity.EmailAddress;
			model.PhoneNumber = entity.PhoneNumber;
			model.MobileNumber = entity.MobileNumber;
			model.WebAddress = entity.WebAddress;		
			model.IsFavourite = entity.IsFavourite;

            model.StreetId = entity.StreetId;
            model.House = entity.House;
            model.Street = entity.Street;
            model.Suburb = entity.Suburb;
            model.Town = entity.Town;
            model.Province = entity.Province;
            model.Country = entity.Country;

            return model;
        }

        /// <summary>
        /// To the model collection.
        /// </summary>
        /// <param name="dtoList">The dto list.</param>
        /// <returns></returns>
        internal static List<ModelProvider> ToModelCollection(this List<EntityProvider> dtoList)
        {
            var modelList = new List<ModelProvider>();

            if (dtoList == null)
                return modelList;

            foreach (var dto in dtoList)
            {
                modelList.Add(dto.ToModel());
            }

            return modelList;
        }

        /// <summary>
        /// To the entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        internal static EntityProvider ToEntity(this ModelProvider model)
        {
            var entity = new EntityProvider();
			entity.Id = model.Id;
			entity.Name = model.Name;
			entity.ContactPerson = model.ContactPerson;
			entity.EmailAddress = model.EmailAddress;
			entity.PhoneNumber = model.PhoneNumber;
			entity.MobileNumber = model.MobileNumber;
			entity.WebAddress = model.WebAddress;
			entity.StreetId = model.StreetId;			
			entity.IsFavourite = model.IsFavourite;

            entity.StreetId = model.StreetId;
            entity.House = model.House;
            entity.Street = model.Street;
            entity.Suburb = model.Suburb;
            entity.Town = model.Town;
            entity.Province = model.Province;
            entity.Country = model.Country;

            return entity;
        }

        /// <summary>
        /// To the entity collection.
        /// </summary>
        /// <param name="dtoList">The dto list.</param>
        /// <returns></returns>
        internal static List<EntityProvider> ToEntityCollection(this List<ModelProvider> dtoList)
        {
            var modelList = new List<EntityProvider>();

            if (dtoList == null)
                return modelList;

            foreach (var dto in dtoList)
            {
                modelList.Add(dto.ToEntity());
            }

            return modelList;
        }

        #endregion

        #region Installer

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        internal static ModelInstaller ToModel(this EntityInstaller entity)
        {
            var model = new ModelInstaller();
            model.Id = entity.Id;		
			model.Name = entity.Name;
			model.ContactPerson = entity.ContactPerson;
			model.EmailAddress = entity.EmailAddress;
			model.PhoneNumber = entity.PhoneNumber;
			model.MobileNumber = entity.MobileNumber;
			model.WebAddress = entity.WebAddress;
			model.StreetId = entity.StreetId;			
			model.IsFavourite = entity.IsFavourite;
            model.StreetId = entity.StreetId;
            model.House = entity.House;
            model.Street = entity.Street;
            model.Suburb = entity.Suburb;
            model.Town = entity.Town;
            model.Province = entity.Province;
            model.Country = entity.Country;

            return model;
        }

        /// <summary>
        /// To the model collection.
        /// </summary>
        /// <param name="dtoList">The dto list.</param>
        /// <returns></returns>
        internal static List<ModelInstaller> ToModelCollection(this List<EntityInstaller> dtoList)
		{
			var modelList = new List<ModelInstaller>();

			if (dtoList == null)
				return modelList;

			foreach (var dto in dtoList)
			{
				modelList.Add(dto.ToModel());
			}

			return modelList;
		}

        /// <summary>
        /// To the entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        internal static EntityInstaller ToEntity(this ModelInstaller model)
		{
			var entity = new EntityInstaller();
			entity.Id = model.Id;
			entity.Name = model.Name;
			entity.ContactPerson = model.ContactPerson;
			entity.EmailAddress = model.EmailAddress;
			entity.PhoneNumber = model.PhoneNumber;
			entity.MobileNumber = model.MobileNumber;
			entity.WebAddress = model.WebAddress;
			entity.StreetId = model.StreetId;			
			entity.IsFavourite = model.IsFavourite;

            entity.StreetId = model.StreetId;
            entity.House = model.House;
            entity.Street = model.Street;
            entity.Suburb = model.Suburb;
            entity.Town = model.Town;
            entity.Province = model.Province;
            entity.Country = model.Country;

            return entity;
		}

        /// <summary>
        /// To the entity collection.
        /// </summary>
        /// <param name="dtoList">The dto list.</param>
        /// <returns></returns>
        internal static List<EntityInstaller> ToEntityCollection(this List<ModelInstaller> dtoList)
		{
			var modelList = new List<EntityInstaller>();

			if (dtoList == null)
				return modelList;

			foreach (var dto in dtoList)
			{
				modelList.Add(dto.ToEntity());
			}

			return modelList;
		}

        #endregion

        #region ProviderPayment

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        internal static ModelProviderPayment ToModel(this EntityProviderPayment entity)
		{
			var model = new ModelProviderPayment();
			model.Id = entity.Id;
			model.ProductId = entity.ProductId;
			model.ProviderId = entity.ProviderId;
			model.AmountPaid = entity.AmountPaid;
			model.DatePaid = entity.DatePaid;
            model.ReceiptNumber = entity.ReceiptNumber;

            return model;
		}

        /// <summary>
        /// To the model collection.
        /// </summary>
        /// <param name="dtoList">The dto list.</param>
        /// <returns></returns>
        internal static List<ModelProviderPayment> ToModelCollection(this List<EntityProviderPayment> dtoList)
        {
            var modelList = new List<ModelProviderPayment>();

            if (dtoList == null)
                return modelList;

            foreach (var dto in dtoList)
            {
                modelList.Add(dto.ToModel());
            }

            return modelList;
        }

        /// <summary>
        /// To the entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        internal static EntityProviderPayment ToEntity(this ModelProviderPayment model)
		{
            var entity = new EntityProviderPayment();
            entity.Id = model.Id;
            entity.ProductId = model.ProductId;
            entity.ProviderId = model.ProviderId;
            entity.AmountPaid = model.AmountPaid;
            entity.DatePaid = model.DatePaid;
            entity.ReceiptNumber = model.ReceiptNumber;

            return entity;
        }

        #endregion

        #region Product

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        internal static ModelProduct ToModel(this EntityProduct entity)
        {
            var model = new ModelProduct();
            model.Id = entity.Id;
			model.Name = entity.Name;
			model.Description = entity.Description;
			model.Price = entity.Price;

			return model;
        }

        /// <summary>
        /// To the model collection.
        /// </summary>
        /// <param name="dtoList">The dto list.</param>
        /// <returns></returns>
        internal static List<ModelProduct> ToModelCollection(this List<EntityProduct> dtoList)
        {
            var modelList = new List<ModelProduct>();

            if (dtoList == null)
                return modelList;

            foreach (var dto in dtoList)
            {
                modelList.Add(dto.ToModel());
            }

            return modelList;
        }

        /// <summary>
        /// To the entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        internal static EntityProduct ToEntity(this ModelProduct model)
        {
            var entity = new EntityProduct();
            entity.Id = model.Id;
			entity.Name = model.Name;
			entity.Description = model.Description;
			entity.Price = model.Price;
			
			return entity;
        }

        /// <summary>
        /// To the entity collection.
        /// </summary>
        /// <param name="dtoList">The dto list.</param>
        /// <returns></returns>
        internal static List<EntityProduct> ToEntityCollection(this List<ModelProduct> dtoList)
        {
            var modelList = new List<EntityProduct>();

            if (dtoList == null)
                return modelList;

            foreach (var dto in dtoList)
            {
                modelList.Add(dto.ToEntity());
            }

            return modelList;
        }

        #endregion

        #region ProviderProduct

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        internal static ModelProviderProduct ToModel(this EntityProviderProduct entity)
        {
            var model = new ModelProviderProduct();
            model.Id = entity.Id;

            return model;
        }

        /// <summary>
        /// To the model collection.
        /// </summary>
        /// <param name="dtoList">The dto list.</param>
        /// <returns></returns>
        internal static List<ModelProviderProduct> ToModelCollection(this List<EntityProviderProduct> dtoList)
        {
            var modelList = new List<ModelProviderProduct>();

            if (dtoList == null)
                return modelList;

            foreach (var dto in dtoList)
            {
                modelList.Add(dto.ToModel());
            }

            return modelList;
        }

        /// <summary>
        /// To the entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        internal static EntityProviderProduct ToEntity(this ModelProviderProduct model)
        {
            var entity = new EntityProviderProduct();
            entity.Id = model.Id;
           

            return entity;
        }

        /// <summary>
        /// To the entity collection.
        /// </summary>
        /// <param name="dtoList">The dto list.</param>
        /// <returns></returns>
        internal static List<EntityProviderProduct> ToEntityCollection(this List<ModelProviderProduct> dtoList)
        {
            var modelList = new List<EntityProviderProduct>();

            if (dtoList == null)
                return modelList;

            foreach (var dto in dtoList)
            {
                modelList.Add(dto.ToEntity());
            }

            return modelList;
        }

        #endregion

        #region Province

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        internal static ModelProvince ToModel(this EntityProvince entity)
		{
			var model = new ModelProvince();
			model.Id = entity.Id;
			model.Name = entity.Name;
			model.Country = entity.Country;

			return model;
		}

        /// <summary>
        /// To the model collection.
        /// </summary>
        /// <param name="dtoList">The dto list.</param>
        /// <returns></returns>
        internal static List<ModelProvince> ToModelCollection(this List<EntityProvince> dtoList)
		{
			var modelList = new List<ModelProvince>();

			if (dtoList == null)
				return modelList;

			foreach (var dto in dtoList)
			{
				modelList.Add(dto.ToModel());
			}

			return modelList;
		}

        #endregion

        #region Town

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        internal static ModelTown ToModel(this EntityTown entity)
		{
			var model = new ModelTown();
			model.Id = entity.Id;
            model.ProvinceId = entity.ProvinceId;
            model.Name = entity.Name;

			return model;
		}

        /// <summary>
        /// To the model collection.
        /// </summary>
        /// <param name="dtoList">The dto list.</param>
        /// <returns></returns>
        internal static List<ModelTown> ToModelCollection(this List<EntityTown> dtoList)
		{
			var modelList = new List<ModelTown>();

			if (dtoList == null)
				return modelList;

			foreach (var dto in dtoList)
			{
				modelList.Add(dto.ToModel());
			}

			return modelList;
		}

        #endregion

        #region Suburb	

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        internal static ModelSuburb ToModel(this EntitySuburb entity)
		{
			var model = new ModelSuburb();
			model.Id = entity.Id;
            model.TownId = entity.TownId;
            model.Name = entity.Name;

			return model;
		}

        /// <summary>
        /// To the model collection.
        /// </summary>
        /// <param name="dtoList">The dto list.</param>
        /// <returns></returns>
        internal static List<ModelSuburb> ToModelCollection(this List<EntitySuburb> dtoList)
        {
            var modelList = new List<ModelSuburb>();

            if (dtoList == null)
                return modelList;

            foreach (var dto in dtoList)
            {
                modelList.Add(dto.ToModel());
            }

            return modelList;
        }

        #endregion

        #region Street		

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        internal static ModelStreet ToModel(this EntityStreet entity)
		{
			var model = new ModelStreet();
			model.Id = entity.Id;
            model.SuburbId = entity.SuburbId;
            model.Name = entity.Name;

			return model;
		}

        /// <summary>
        /// To the model collection.
        /// </summary>
        /// <param name="dtoList">The dto list.</param>
        /// <returns></returns>
        internal static List<ModelStreet> ToModelCollection(this List<EntityStreet> dtoList)
		{
			var modelList = new List<ModelStreet>();

			if (dtoList == null)
				return modelList;

			foreach (var dto in dtoList)
			{
				modelList.Add(dto.ToModel());
			}

			return modelList;
		}

        #endregion

        #region Application

        /// <summary>
        /// To the model collection.
        /// </summary>
        /// <param name="dtoList">The dto list.</param>
        /// <returns></returns>
        internal static List<ModelApplication> ToModelCollection(this List<EntityApplication> dtoList)
        {
            var modelList = new List<ModelApplication>();

            if (dtoList == null)
                return modelList;

            foreach (var dto in dtoList)
            {
                modelList.Add(dto.ToModel());
            }

            return modelList;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        internal static ModelApplication ToModel(this EntityApplication entity)
        {
            return new ModelApplication
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                PhoneNumber = entity.PhoneNumber,
                MobileNumber = entity.MobileNumber,
                EmailAddress = entity.EmailAddress,
                Street = entity.Street,
                Suburb = entity.Suburb,
                Town = entity.Town,
                Province = entity.Province,
                Country = entity.Country,
                PostalCode = entity.PostalCode                 
            };
        }

        /// <summary>
        /// To the entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        internal static EntityApplication ToEntity(this ModelApplication model)
        {
            return new EntityApplication
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                MobileNumber = model.MobileNumber,
                EmailAddress = model.EmailAddress,
                Street = model.Street,
                Suburb = model.Suburb,
                Town = model.Town,
                Province = model.Province,
                Country = model.Country,
                PostalCode = model.PostalCode
            };
        }

        #endregion
    }
}
