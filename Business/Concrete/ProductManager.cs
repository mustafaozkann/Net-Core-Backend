﻿using Business.Abstract;
using Business.Constants;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Business.BusinessAspects.Autofac;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.CrossCuttingConcerns.Validation;
using Core.Extensions;
using Core.Utilities.Results;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        [LogAspect(typeof(DatabaseLogger))]
        [ValidationAspect(typeof(ProductValidator), Priority = 1)]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {
            _productDal.Add(product);
            return new SuccessResult(message: Message.ProductAdded);
        }

        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new SuccessResult(message: Message.ProductDeleted);
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(filter: x => x.ProductId == productId));
        }

        [PerformanceAspect(5)]
        public IDataResult<List<Product>> GetList()
        {
            Thread.Sleep(5000);
            return new SuccessDataResult<List<Product>>(_productDal.GetList().ToList());
        }

        //[SecuredOperation("Product.List,Admin")]
        [LogAspect(typeof(DatabaseLogger))]
        [CacheAspect(duration: 10)]
        public IDataResult<List<Product>> GetListByCategory(int categoryId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetList(filter: x => x.CategoryId == categoryId).ToList());
        }

        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult(message: Message.ProductUpdated);
        }

        [TransactionScopeAspect]
        public IResult TransactionalOperation(Product product)
        {
            _productDal.Update(product);
            _productDal.Add(product);
            return new SuccessResult(message: Message.ProductUpdated);

        }
    }
}
