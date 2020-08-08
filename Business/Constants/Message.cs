﻿using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public static class Message
    {
        public static string ProductAdded = "Ürün başarıyla eklendi.";
        public static string ProductDeleted = "Ürün başarıyla silindi.";
        public static string ProductUpdated = "Ürün başarıyla güncellendi.";


        public static string UserNotFound = "Boyle bir kullanici sistemde bulunamadı.";

        public static string PasswordError = "Şifre hatalı.";

        public static string SuccessfulLogin = "Sisteme giriş başarılı.";

        public static string UserAlreadyExists = "Bu kullanıcı zaten mevcut.";

        public static string UserRegistered = "Kullanıcı başarıyla kaydedildi.";

        public static string AccessTokenCreated = "Access token başarıyla oluşturuldu.";
    }
}