
var CommonValidation = {
    Rules: {
        Name: {
            required: true,
            maxlength: 50
        },
        Sort:
        {
            required: true,
            digits: true,
            min: 0,
            max: 99999
        }
    },
    Messages:
    {
        Name:
        {
            required: "请输入名称",
            maxlength: "名称长度不能超过50个字符"
        },
        Sort: {
            required: "请输入一个整数排序",
            digits: "排序只允许正整数",
            min: "排序不能小于零",
            max: "排序最大不能超过99999"
        }
    }
};


var $$validations = {
    Role: {
        Rules: {
            Name: {
                required: true,
                maxlength: 50
            },
            Description: {
                maxlength: 500
            }
        },
        Messages:
        {
            Name:
            {
                required: "请输入名称",
                maxlength: "名称长度不能超过50个字符"
            },
            Description:
            {
                maxlength: "描述长度不能超过500个字符"
            }
        }
    },
    Administrator: {
        Rules: {
            LogonName: {
                required: true,
                maxlength: 20
            },
            Email: {
                required: true,
                email: true,
                maxlength: 50
            },
            Password: {
                required: true,
                minlength: 6
            },
            ConfirmPassword: {
                required: true,
                equalTo: "#Password"
            },
            Name: {
                required: true,
                maxlength: 50
            },
            Phone: {
                maxlength: 20
            },
            QQ: {
                digits: true,
                maxlength: 25
            }
        },
        Messages: {
            LogonName: {
                required: "请输入登录帐号",
                maxlength: "登录帐号长度不能超过20"
            },
            Email: {
                required: "请输入邮箱",
                email: "邮箱地址无效",
                maxlength: "邮箱长度不能超过50"
            },
            Password: {
                required: "请输入密码",
                minlength: "登录密码最小长度是6位"
            },
            ConfirmPassword: {
                required: "请再次输入密码",
                equalTo: "请确保与上面的密码输入一致"
            },
            Name: {
                required: "请输入真实姓名",
                maxlength: "真实姓名长度不能超过50个字符"
            },
            Phone: {
                maxlength: "手机号码长度不能超过20个字符"
            },
            QQ: {
                digits: "QQ号码只允许数字",
                maxlength: "该字段长度不能超过25个数字"
            }
        }
    },
    UpdatePassword: {
        Rules: {
            OldPassword: {
                required: true
            },
            NewPassword: {
                required: true,
                minlength: 6
            },
            ConfirmPassword: {
                required: true,
                equalTo: "#NewPassword"
            }
        },
        Messages:
        {
            OldPassword:
            {
                required: "请输入当前的登录密码"
            },
            NewPassword:
            {
                required: "请输入新的登录密码",
                minlength: "登录密码最小长度是6位"
            },
            ConfirmPassword:
            {
                required: "请再次输入新登录密码",
                equalTo: "请确保与新密码的输入一致"
            }
        }
    },
    Language: {
        Rules: {
            Name: {
                required: true,
                maxlength: 50
            },
            Sort:
            {
                required: true,
                digits: true,
                min: 0,
                max: 99999
            },
            CurrencyId: {
                required: true
            }
        },
        Messages: {
            Name: {
                required: "请输入名称",
                maxlength: "名称长度不能超过50个字符"
            },
            Sort: {
                required: "请输入一个整数排序",
                digits: "排序只允许正整数",
                min: "排序不能小于零",
                max: "排序最大不能超过99999"
            },
            CurrencyId: {
                required: "请选择货币"
            }
        }
    },
    Article: {
        Rules: {
            PageId: {
                required: true
            },
            PublishStart: {
                required: true
            },
            Name: {
                required: true,
                maxlength: 50
            },
            PageTitle: {
                required: true,
                maxlength: 200
            },
            Sort: {
                required: true,
                digits: true,
                min: 0,
                max: 99999
            },
            Views: {
                required: true,
                digits: true,
                min: 0
            }
        },
        Messages: {
            PageId: {
                required: "请选择文章所属栏目"
            },
            PublishStart: {
                required: "请选择文章发布日期"
            },
            Name: {
                required: "请输入名称",
                maxlength: "名称长度不能超过50个字符"
            },
            PageTitle: {
                required: "请输入页面标题",
                maxlength: "页面标题长度不能超过200个字符"
            },
            Sort: {
                required: "请输入一个整数排序",
                digits: "排序只允许正整数",
                min: "排序不能小于零",
                max: "排序最大不能超过99999"
            },
            Views: {
                required: "请输入一个整数",
                digits: "浏览量只允许正整数",
                min: "浏览量不能小于零"
            }
        }
    },
    Page: {
        Rules: {
            Name: {
                required: true,
                maxlength: 50
            },
            PageTitle: {
                required: true,
                maxlength: 200
            },
            Sort: {
                required: true,
                digits: true,
                min: 0,
                max: 99999
            },
            UrlIndex: {
                required: function (me) {
                    var pageType = $(me).parents("form").find("select[name='PageType']").val();

                    if (pageType == 10) { // 单页图文
                        return true;
                    } else {
                        return false;
                    }

                },
                maxlength: 200
            },
            Url: {
                required: function (me) {
                    var pageType = $(me).parents("form").find("select[name='PageType']").val();

                    if (pageType == 20) { // 连接
                        return true;
                    } else {
                        return false;
                    }

                },
                maxlength: 200
            }
        },
        Messages: {
            Name: {
                required: "请输入名称",
                maxlength: "名称长度不能超过50个字符"
            },
            PageTitle: {
                required: "请输入页面标题",
                maxlength: "页面标题长度不能超过200个字符"
            },
            Sort: {
                required: "请输入一个整数排序",
                digits: "排序只允许正整数",
                min: "排序不能小于零",
                max: "排序最大不能超过99999"
            },
            UrlIndex: {
                required: "请输入UrlIndex",
                maxlength: "UrlIndex长度不能超过200个字符"
            },
            Url: {
                required: "请输入Url",
                maxlength: "Url长度不能超过200个字符"
            }
        }
    },
    Attribute: {
        Rules: {
            AttributeTpye: {
                required: true
            },
            Name: {
                required: true,
                maxlength: 50
            },
            Sort: {
                required: true,
                digits: true,
                min: 0,
                max: 99999
            },
            DefaultValue: {
                dfIsNumber: true
                //number: function (me) {
                //    var type = $(me).parents("form").find("select[name='AttributeTpye']").val();      
                //    var isnumber = false;
                //    if (type == 20) { // 数字 {
                //        var v = $(me).val();
                //        console.log(v);
                //        if (util.regex(v, /^-?(?:\d+|\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/))
                //        {
                //            //isnumber = true;
                //        }                        
                //    }

                //    return isnumber;
                //}
            },
            OptionName: {
                required: function (me) {
                    var type = $(me).parents("form").find("select[name='AttributeTpye']").val();

                    var isRequired = false;
                    if (type == 40 || type == 50) {
                        isRequired = true;
                    }
                    return isRequired;
                }
            }
        },
        Messages: {
            AttributeTpye: {
                required: "请选择属性类型"
            },
            Name: {
                required: "请输入名称",
                maxlength: "名称长度不能超过50个字符"
            },
            Sort: {
                required: "请输入一个整数排序",
                digits: "排序只允许正整数",
                min: "排序不能小于零",
                max: "排序最大不能超过99999"
            },
            DefaultValue: {
                number: "请输入数字"
            },
            OptionName: {
                required: "请输入选项"
            }
        }
    },
    Brand: {
        Rules: {
            Name: {
                required: true,
                maxlength: 50
            },
            PageTitle: {
                required: true,
                maxlength: 200
            },
            Sort: {
                required: true,
                digits: true,
                min: 0,
                max: 99999
            },
            ProductTypeId: {
                required: true
            }
        },
        Messages: {
            Name: {
                required: "请输入名称",
                maxlength: "名称长度不能超过50个字符"
            },
            PageTitle: {
                required: "请输入页面标题",
                maxlength: "页面标题长度不能超过200个字符"
            },
            Sort: {
                required: "请输入一个整数排序",
                digits: "排序只允许正整数",
                min: "排序不能小于零",
                max: "排序最大不能超过99999"
            },
            ProductTypeId: {
                required: "请选择关联的商品类型"
            }
        }
    },
    ProductType: {
        Rules: {
            Name: {
                required: true,
                maxlength: 50
            },
            PageTitle: {
                required: true,
                maxlength: 200
            },
            Sort: {
                required: true,
                digits: true,
                min: 0,
                max: 99999
            }
        },
        Messages: {
            Name: {
                required: "请输入名称",
                maxlength: "名称长度不能超过50个字符"
            },
            PageTitle: {
                required: "请输入页面标题",
                maxlength: "页面标题长度不能超过200个字符"
            },
            Sort: {
                required: "请输入一个整数排序",
                digits: "排序只允许正整数",
                min: "排序不能小于零",
                max: "排序最大不能超过99999"
            }
        }
    },
    Category: {
        Rules: {
            Name: {
                required: true,
                maxlength: 50
            },
            PageTitle: {
                required: true,
                maxlength: 200
            },
            Sort: {
                required: true,
                digits: true,
                min: 0,
                max: 99999
            },
            ProductTypeId: {
                required: true
            }
        },
        Messages: {
            Name: {
                required: "请输入名称",
                maxlength: "名称长度不能超过50个字符"
            },
            PageTitle: {
                required: "请输入页面标题",
                maxlength: "页面标题长度不能超过200个字符"
            },
            Sort: {
                required: "请输入一个整数排序",
                digits: "排序只允许正整数",
                min: "排序不能小于零",
                max: "排序最大不能超过99999"
            },
            ProductTypeId: {
                required: "请选择类别"
            }
        }
    },
    ProductTag: {
        Rules: {
            Name: {
                required: true,
                maxlength: 50
            },
            PageTitle: {
                required: true,
                maxlength: 200
            },
            Sort: {
                required: true,
                digits: true,
                min: 0,
                max: 99999
            }
        },
        Messages: {
            Name: {
                required: "请输入名称",
                maxlength: "名称长度不能超过50个字符"
            },
            PageTitle: {
                required: "请输入页面标题",
                maxlength: "页面标题长度不能超过200个字符"
            },
            Sort: {
                required: "请输入一个整数排序",
                digits: "排序只允许正整数",
                min: "排序不能小于零",
                max: "排序最大不能超过99999"
            }
        }
    },
    ProductSpecification: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            Sort: CommonValidation.Rules.Sort,
            Alias: {
                maxlength: 50
            },           
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            Sort: CommonValidation.Messages.Sort,
            Alias: {
                maxlength: "别名长度不能超过50个字符"
            },
        }
    },
    Vacancy: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            Sort: CommonValidation.Rules.Sort,
            PublishStart: {
                required: true
            },
            Quantity: {
                digits: true,
                min: 0,
                max: 9999
            },
            Salary: {
                min: 0,
                digits: true
            }
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            Sort: CommonValidation.Messages.Sort,
            PublishStart: {
                required: "请选择文章发布日期"
            },
            Quantity: {
                digits: "请输入一个整数",
                min: "招聘人数必须大于等于0",
                max: "招聘人数最大不能超过9999"
            },
            Salary: {
                min: "必须大于等于0",
                digits: "请输入一个整数",
            }
        }
    },

    FriendlyLink: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            Sort: CommonValidation.Rules.Sort,
            Url: {
                required: true,
                url: true,
                maxlength: 500
            }
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            Sort: CommonValidation.Messages.Sort,
            Url: {
                required: "请输入URL",
                url: "请输入合法URL",
                maxlength: "URL长度不能超过500字符",
            }
        }
    },

    BannerGroup: {
        Rules: CommonValidation.Rules,
        Messages: CommonValidation.Messages
    },
    Banner: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            Sort: CommonValidation.Rules.Sort,
            BannerGroupId: {
                required: true
            },
            ImageUrl: {
                required: true,
            }       
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            Sort: CommonValidation.Messages.Sort,

            BannerGroupId: {
                required: "请选择图片所属轮播组",
            },
            ImageUrl: {
                required: "请选择图片"
            }        
        }
    },

    RegexValidation: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            Regex: {
                required: true
            },
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            Regex: {
                required: "请输入正则表达式",
            },
        }
    },
    DownLoadGroup: {
        Rules: CommonValidation.Rules,
        Messages: CommonValidation.Messages
    },
    DownLoad: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            Sort: CommonValidation.Rules.Sort,
            DownLoadGroupId: {
                required: true
            },
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            Sort: CommonValidation.Messages.Sort,
            DownLoadGroupId: {
                required: "请选择文件所属的文件组",
            },          
        }
    },

    DataDictionary: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            Sort: CommonValidation.Rules.Sort,
            DataValue: {
                required: true
            },
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            Sort: CommonValidation.Messages.Sort,
            DataValue: {
                required: "请输入常用数据的值",
            },
        }
    },

    DataTemplate: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            Sort: CommonValidation.Rules.Sort,
            Content: {
                required: true
            },
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            Sort: CommonValidation.Messages.Sort,
            Content: {
                required: "请输入模板内容",
            },
        }
    },


    Branches: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            Sort: CommonValidation.Rules.Sort,

            Telephone: {
                required: true,
            },
            Lng: {
                required: true,
            },
            Lat: {
                required: true,
            },
            Address: {
                required: true,
            }
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            Sort: CommonValidation.Messages.Sort,
           
            Telephone: {
                required: "请输入联系电话",
            },
            Lng: {
                required: "请选择门店点位以确定经度",
            },
            Lat: {
                required: "请选择门店点位以确定纬度",
            },
            Address: {
                required: "请输入地址",
            }
        }
    },
    Appointment: {
        Rules: {
          
            BranchesId: {
                required: true
            },
            UserName: {
                required: true,
            },
            UserMobile: {
                required: true,
            },
            AppointDate: {
                required: true,
            },
          
        },
        Messages: {
            BranchesId: {
                required: "请选择一个门店"
            },
            UserName: {
                required: "请输入预约用户姓名",

            },
            UserMobile: {
                required: "请输入用户手机号码",
            },
            AppointDate: {
                required: "请选择预约日期",
            },
        }
    },

    EmailBox: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            Sort: CommonValidation.Rules.Sort,
            Email: {
                required: true,
                email: true,
                maxlength: 50
            },
            Password: {
                required: true,
                minlength: 6
            },
            ConfirmPassword: {
                required: true,
                equalTo: "#Password"
            },
            SMTP: {
                required: true,
                maxlength: 50
            },
            Port: {
                required: true,
                digits: true,
            }            
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            Sort: CommonValidation.Messages.Sort,
            Email: {
                required: "请输入Email",
                email: "请输入正确格式Email",
                maxlength: "Email长度过长",
            },
            Password: {
                required: "请输入密码",
                minlength: "密码不得短于6位",
            },
            ConfirmPassword: {
                required: "请再次输入密码",
                equalTo: "两次密码输入不一致",
            },
            SMTP: {
                required: "请输入SMTP",
                maxlength: "SMTP过长",
            },
            Port: {
                required: "请输入端口号",
                digits: "端口号必须为数字",
            }
        }
    },

    ChangeEmailPassword: {
        Rules: {           
            OldPassword: {
                required: true,
                minlength: 6
            },
            Password: {
                required: true,
                minlength: 6
            },
            ConfirmPassword: {
                required: true,
                equalTo: "#Password"
            },
        },
        Messages: {
            OldPassword: {
                required: "请输入旧密码",
                minlength: "密码不得短于6位",
            },
            Password: {
                required: "请输入新密码",
                minlength: "密码不得短于6位",
            },

            ConfirmPassword: {
                required: "请再次输入新密码",
                equalTo: "两次密码输入不一致",
            },
        }
    },    

    UserLevel: {
        Rules: {
            Name: CommonValidation.Rules.Name,

            PointRequired: {
                required: true,
                digits: true,
                min: 0,
            },
            Discount: {
                required: true,
                digits: true,
                min: 0,
                max: 100
            }
        },
        Messages: {
            Name: CommonValidation.Messages.Name,

            PointRequired: {
                required: "请输入所需积分",
                digits: "所需积分必须是整数",
                min: "所需积分必须大于等于0",
            },
            Discount: {
                required: "请输入该等级优惠折扣",
                digits: "优惠折扣请填写0-100的数字",
                min: "优惠折扣请填写0-100的数字",
                max: "优惠折扣请填写0-100的数字",
            },
          
        }
    },
    User: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            UserLevelId: { required: true },
            Language: {
                required: true
            },
            Point: {
                required: true,
                digits: true,
                min: 0,
            },
            LanguageId: {
                required: true,
            },
            Password: {
                required: true,
                minlength: function () {
                    var lenght = $("#UserCenterConfig_MinPassLen").val();
                  
                    return parseInt(lenght);
                } 
            },
            ConfirmPassword: {
                required: true,
                equalTo: "#Password"
            },
            Email: {
                email: true
            },

        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            UserLevelId: {
                required: "请选择用户等级"
            },
            Language: {
                required: "请选择语言"
            },
            Point: {
                required: "请输入所需积分",
                digits: "所需积分必须是整数",
                min: "所需积分必须大于等于0",
            },
            LanguageId: {
                required: "请选择用户使用的语言",
            },
            Password: {
                required: "请输入密码",
                minlength: "密码长度过短",
            },
            ConfirmPassword: {
                required: "请再次输入密码",
                equalTo: "两次密码输入不一致",
            },
            Email: {
                email: "请输入格式正确的Email地址"
            },
        }
    },
    
    Product: {
        Rules: {
            ValuationMethod: {
                required: true
            },
            ProductTypeId: {
                required: true
            },
            BrandId: {
                required: true
            },
            PageTitle: {
                required: true,
                maxlength: 200
            },
            MoldId: {
                required: true
            },
            CategoryId: {
                required: true
            },
            Name: {
                required: true,
                maxlength: 50
            },
            Sort: {
                required: true,
                digits: true,
                min: 0,
                max: 99999
            },
            Price: {
                required: true,
                number: true,
                min: 0,
                max: 999999999.99
            },
            MarketPrice: {               
                number: true,
                min: 0,
                max: 999999999.99
            },
            PromotionPrice: {
                number: true,
                min: 0,
                max: 999999999.99
            },
            CountOfStars: {
                digits: true,
                min: 1,
                max: 5
            },
            CountOfSold: {
                digits: true,
                min: 0,
                max: 9999999999
            },
            CountOfCollected: {
                digits: true,
                min: 0,
                max: 9999999999
            },
            CountOfViews: {
                digits: true,
                min: 0,
                max: 9999999999
            },
            Credits: {
                digits: true,
                min: 0,
                max: 9999999999
            },
            Store: {
                digits: true,
                min: 0,
                max: 9999999999
            },
            Weight: {
                number: true,
                min: 0,
                max: 99999999.99
            }
        },
        Messages: {
            ValuationMethod: {
                required: "请选择商品定价类型"
            },
            ProductTypeId: {
                required: "请选择商品类别"
            },
            BrandId: {
                required: "请选择商品品牌"
            },
            CategoryId: {
                required: "请选择商品分类"
            },
            MoldId: {
                required: "请选择商品类型"
            },
            Name: {
                required: "请输入名称",
                maxlength: "名称长度不能超过50个字符"
            },
            PageTitle: {
                required: "请输入页面标题",
                maxlength: "页面标题长度不能超过200个字符"
            },
            Sort: {
                required: "请输入一个整数排序",
                digits: "只允许正整数",
                min: "不能小于零",
                max: "排序最大不能超过99999"
            },
            Price: {
                required: "请输入价格",
                number: "请输入有效数字",
                min: "价格不能小于0",
                max: "价格不能大于999999999.99"
            },
            MarketPrice: {
                number: "请输入有效数字",
                min: "不能小于0",
                max: "不能大于999999999.99"
            },
            PromotionPrice: {
                number: "请输入有效数字",
                min: "不能小于0",
                max: "不能大于999999999.99"
            },
            CountOfStars: {
                digits: "请输入1-5的正整数",
                min: "不能小于1",
                max: "不能大于5"
            },
            CountOfSold: {
                digits: "请输入正整数",
                min: "不能小于0",
                max: "不能大于9999999999"
            },
            CountOfCollected: {
                digits: "请输入正整数",
                min: "不能小于0",
                max: "不能大于9999999999"
            },
            CountOfViews: {
                digits: "请输入正整数",
                min: "不能小于0",
                max: "不能大于9999999999"
            },
            Credits: {
                digits: "请输入正整数",
                min: "不能小于0",
                max: "不能大于9999999999"
            },
            Store: {
                digits: "请输入正整数",
                min: "不能小于0",
                max: "不能大于9999999999"
            },
            Weight: {
                number: "请输入有效数字",
                min: "不能小于0",
                max: "不能大于999999999.99"
            }
        }
    },
    ProductEnquery: {
        Rules: {           
            ProductId: {
                required: true
            },
            CreatedOn: {
                required: true
            },
            Enquery: {
                required: true
            }
        },
        Messages: {
            ProductId: {
                required: "请选择商品"
            },
            CreatedOn: {
                required: "请选择咨询时间"
            },
            Enquery: {
                required: "请输入咨询内容"
            }
        }
    },
    ProductComment: {
        Rules: {
            ProductGoodsId: {
                required: true
            },
            CreatedOn: {
                required: true
            },
            Comment: {
                required: true
            }
        },
        Messages: {
            ProductGoodsId: {
                required: "请选择货品"
            },
            CreatedOn: {
                required: "请选择评价时间"
            },
            Comment: {
                required: "请输入评价内容"
            }
        }
    },
    VCategory: {
        Rules: {
            Name: {
                required: true,
                maxlength: 50
            },
            PageTitle: {
                required: true,
                maxlength: 200
            },
            Sort: {
                required: true,
                digits: true,
                min: 0,
                max: 99999
            },
            PriceMin: {
                min: 0,
                max: 999999999.99
            },
            PriceMax: {
                min: 0,
                max: 999999999.99
            }
        },
        Messages: {
            Name: {
                required: "请输入名称",
                maxlength: "名称长度不能超过50个字符"
            },
            PageTitle: {
                required: "请输入页面标题",
                maxlength: "页面标题长度不能超过200个字符"
            },
            Sort: {
                required: "请输入一个整数排序",
                digits: "排序只允许正整数",
                min: "排序不能小于零",
                max: "排序最大不能超过99999"
            },
            PriceMin: {
                min: "不能小于零",
                max: "最大不能超过999999999"
            },
            PriceMax: {
                min: "不能小于零",
                max: "最大不能超过999999999"
            }
        }
    },
    Coupon: {
        Rules: {
            Name: {
                required: true,
                maxlength: 50
            },
            No: {
                required: true,
                maxlength: 50
            },
            Type: {
                required: true
            },
            RuleType: {
                required: true
            },
            Count: {
                digits: true,
                min: 1,
                max: 99999999999
            },
            MinAccount: {
                number: true,
                required: true,
                min: 1,
                max: 999999999.99
            },
            StartDate: {
                required: true
            },
            EndDate: {
                required: true
            },
            UserLevelIds: {
                required: true
            },
            Description: {
                required: true
            },
            Discount: {
                required: function (me) {
                    var ruleType = $(me).closest("form").find('input:radio[name="RuleType"]:checked').val();

                    if (ruleType == 20) {
                        return true;
                    } else {
                        return false;
                    }
                },
                number: true,
                min: 1,
                max: 100
            },
        ReliefAmount: {
            required: function (me) {
                var ruleType = $(me).closest("form").find('input:radio[name="RuleType"]:checked').val();

                if (ruleType == 30) {
                    return true;
                } else {
                    return false;
                }
            },
            number: true,
            min: 1,
            max: 9999999
            }
        },
        Messages: {
            Count: {
                digits: "请输入正整数",
                min: "不能小于0",
                max: "不能大于99999999999"
            },
            Name: {
                required: "请输入名称",
                maxlength: "名称长度不能超过50个字符"
            },
            No: {
                required: "请输入编号",
                maxlength: "编号长度不能超过50个字符"
            },
            Type: {
                required: "请选择类型"
            },
            RuleType: {
                required: "请选择使用规则"
            },
            MinAccount: {
                number: "请输入有效数字",
                required: "请输入订单金额",
                min: "订单金额必须大于等于1",
                max: "最大不能超过999999999.99"
            },
            StartDate: {
                required: "请选择优惠卷使用开始时间"
            },
            EndDate: {
                required: "请选择优惠卷使用结束时间"
            },
            UserLevelIds: {
                required: "请选择会员等级"
            },
            Description: {
                required: "请输入优惠券描述"
            },
            Discount: {
                required: "请输入折扣",
                number: "请输入有效数字",
                min: "最小值大于1",
                max: "最大值100"
            },
            ReliefAmount: {
                required: "请输入减免金额",
                number: "请输入有效数字",
                min: "最小值大于1",
                max: "最大值9999999"
            }
        },
    },
    ExpressCompany: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            Code: { required: true },
            Link: {
                url: true,
                maxlength: 500
            }
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            Code: { required: "请输入快递公司代码", },
            Link: {
                url: "请输入合法URL",
                maxlength: "URL长度不能超过500字符",
            }
        }
    },
    
    AfterSaleReason: {
        Rules: {
            ReasonContent: { required: true },
        },
        Messages: {
            ReasonContent: { required: "请输入内容", },
        }
    },
    AfterSale: {
        Rules: {
            DealState: { required: true },
        },
        Messages: {
            DealState: { required: "请选择一个处理状态", },
        }
    },
    
    DeliveryMethod: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            LanguageId: {
                required: true
            },
            FirstWeight: {
                required: true,
                min: 0,
                max: 9999,
            },
            AddedWeight: {
                required: true,
                min: 0,
                max: 9999,
            },
            FeeFirstDefault: {
                min: 0,
                max: 9999,
            },
            FeeAddedDefault: {
                min: 0,
                max: 9999,
            },
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            FirstWeight: {
                required: "不能为空",
                min: "请输入0-9999的数值",
                max: "请输入0-9999的数值",
            },
            AddedWeight: {
                required: "不能为空",
                min: "请输入0-9999的数值",
                max: "请输入0-9999的数值",
            },
            LanguageId: {
                required: "请选择语言"
            },
            FeeFirstDefault: {
                min: "请输入0-9999的数值",
                max: "请输入0-9999的数值",
            },
            FeeAddedDefault: {
                min: "请输入0-9999的数值",
                max: "请输入0-9999的数值",
            }
        }
    },

    Form: {
        Rules: {
            Name: CommonValidation.Rules.Name,

           
        },
        Messages: {
            Name: CommonValidation.Messages.Name,

           

        }
    },
    FormField: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            FieldName: CommonValidation.Rules.Name,
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            FieldName: CommonValidation.Messages.Name,
        }
    },

    Promotion: {
        Rules: {
            Name: {
                required: true,
                maxlength: 200
            },           
            RuleType: {
                required: true
            },
            MaxCredit: {
                required: function (me) {
                    var ruleType = $(me).closest("form").find('input:radio[name="RuleType"]:checked').val();

                    if (ruleType == 10) {
                        return true;
                    } else {
                        return false;
                    }
                },
                digits: true,
                min: 1,
                max: 99999999999
            },
            MinAccount: {
                number: true,
                required: function (me) {
                    var ruleType = $(me).closest("form").find('input:radio[name="RuleType"]:checked').val();

                    if (ruleType == 10 || ruleType == 20 || ruleType == 30 || ruleType == 40 || ruleType == 50) {
                        return true;
                    } else {
                        return false;
                    }
                },
                min: 1,
                max: 999999999.99
            },
            StartDate: {
                required: true
            },
            EndDate: {
                required: true
            },
            UserLevelIds: {
                required: function (me) {
                    var ruleType = $(me).closest("form").find('input:radio[name="RuleType"]:checked').val();

                    if (ruleType == 10 || ruleType == 15 || ruleType == 20 || ruleType == 30 || ruleType == 40 || ruleType == 50) {
                        return true;
                    } else {
                        return false;
                    }
                }
            },
            CouponId: {
                required: function (me) {
                    var ruleType = $(me).closest("form").find('input:radio[name="RuleType"]:checked').val();

                    if (ruleType == 30 || ruleType == 60 || ruleType == 70 || ruleType == 80 || ruleType == 90) {
                        
                        return true;
                    } else {
                        return false;
                    }
                }
            },
            Description: {
                required: true
            },
            Discount: {
                required: function (me) {
                    var ruleType = $(me).closest("form").find('input:radio[name="RuleType"]:checked').val();

                    if (ruleType == 40 || ruleType == 15) {
                        return true;
                    } else {
                        return false;
                    }
                },
                number: true,
                min: 1,
                max: 100
            },
            ReliefAmount: {
                required: function (me) {
                    var ruleType = $(me).closest("form").find('input:radio[name="RuleType"]:checked').val();

                    if (ruleType == 50) {
                        return true;
                    } else {
                        return false;
                    }
                },
                number: true,
                min: 1,
                max: 9999999
            }
        },
        Messages: {
            MaxCredit: {
                required: "请输入允许兑换的最大积分",
                digits: "请输入正整数",
                min: "不能小于0",
                max: "不能大于99999999999"
            },
            Name: {
                required: "请输入名称",
                maxlength: "名称长度不能超过200个字符"
            },
            RuleType: {
                required: "请选择使用规则"
            },
            MinAccount: {
                number: "请输入有效数字",
                required: "请输入订单金额",
                min: "订单金额必须大于等于1",
                max: "最大不能超过999999999.99"
            },
            StartDate: {
                required: "请选择优惠卷使用开始时间"
            },
            EndDate: {
                required: "请选择优惠卷使用结束时间"
            },
            UserLevelIds: {
                required: "请选择会员等级"
            },
            Description: {
                required: "请输入优惠券描述"
            },
            Discount: {
                required: "请输入折扣",
                number: "请输入有效数字",
                min: "最小值大于1",
                max: "最大值100"
            },
            ReliefAmount: {
                required: "请输入减免金额",
                number: "请输入有效数字",
                min: "最小值大于1",
                max: "最大值9999999"
            },
            CouponId: {
                required: "请选择优惠券"
            }
        }
    },
    Currency: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            Code: { required: true },
            Sign: {
                required: true
            },
            PaymentIds: {
                required: true
            }
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            Code: { required: "请输入货币代码", },
            Sign: {
                required: "请输入货币符号"
            },
            PaymentIds: {
                required: "请选择支付方式"
            }
        }
    },
    OrderProductAttribute: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            Sort: CommonValidation.Rules.Sort,
            TypeId: {
                required: true
            }
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            Sort: CommonValidation.Messages.Sort,
            TypeId: {
                required: "请选择类型"
            }
        }
    },
    Order: {
        Rules: {
            LanguageId: {
                required: true
            },
            UserId: {
                required: true
            },
            PaymentId: {
                required: true
            },
            UserDeliveryAddress: {
                required: true
            },
            DeliveryMethodId: {
                required: true
            },
            ProvinceId: {
                required: true
            },
            CityAreaId: {
                required: true
            },
            Address: {
                required: true
            },
            ReceiverName: {
                required: true
            },
            Mobile: {
                required: true
            }
        },
        Messages: {
            LanguageId: {
                required: "请选择语言"
            },
            UserId: {
                required: "请选择会员"
            },
            PaymentId: {
                required: "请选择支付方式"
            },
            UserDeliveryAddress: {
                required: "请选择收货地址"
            },
            DeliveryMethodId: {
                required: "请选择配送方式"
            },
            ProvinceId: {
                required: "请选择省份"
            },
            CityAreaId: {
                required: "请选择城区"
            },
            Address: {
                required: "请选择城区"
            },
            ReceiverName: {
                required: "请输入收件人"
            },
            Mobile: {
                required: "请输入联系电话"
            }
        }
    },
    OrderPayment: {
        Rules: {
            PaymentStatus: {
                required: true
            },
            PaymentId: {
                required: true
            },
            PayAccount: {
                required: true
            },
            Payer: {
                required: true
            }
        },
        Messages: {
            PaymentStatus: {
                required: "请选择支付类型"
            },
            PaymentId: {
                required: "请选择支付方式"
            },
            PayAccount: {
                required: "请输入付款帐号"
            },
            Payer: {
                required: "请输入付款人"
            }
        }
    },
    OrderRefunds: {
        Rules: {
            Amount: {
                required: true,
                min: 0
            },
            Receiver: {
                required: true
            },
            ReceiveAccount: {
                required: true
            },
            PayAccount: {
                required: true
            },
            Payer: {
                required: true
            },
            DeleteCredits:
            {
                min: 0
            }
        },
        Messages: {
            Amount: {
                required: "请输入金额",
                min: "退款金额必须大于0"
            },
            Receiver: {
                required: "请输入收款人"
            },
            ReceiveAccount: {
                required: "请输入帐号"
            },
            PayAccount: {
                required: "请输入付款帐号"
            },
            Payer: {
                required: "请输入付款人"
            },
            DeleteCredits: {
                min: "扣减积分必须大于0"
            }
        }
    },
    OrderDelivery: {
        Rules: {
            Address: {
                required: true
            },
            ReceiverName: {
                required: true
            },
            Mobile: {
                required: true
            }
        },
        Messages: {
            Address: {
                required: "请输入地址"
            },
            ReceiverName: {
                required: "请输入联系人"
            },
            Mobile: {
                required: "请输入手机号码"
            }
        }
    },
    OrderReturnGoods: {
        Rules: {
            Reason: {
                required: true
            },
            ReceiverName: {
                required: true
            },
            Mobile: {
                required: true
            }
        },
        Messages: {
            Reason: {
                required: "请输入退货原因"
            },
            ReceiverName: {
                required: "请输入联系人"
            },
            Mobile: {
                required: "请输入手机号码"
            }
        }
    },
    DataSourceField: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            FieldName: CommonValidation.Rules.Name,
            ForeignDataSourceId: {
                required: function (me) {
                    var pageType = $(me).closest("form").find("#TypeId").val();
                    if (pageType == 70) { // 数据源
                        return true;
                    } else {
                        return false;
                    }

                }
            },
            TypeId: {
                required: true
            }
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            FieldName: CommonValidation.Messages.Name,
            ForeignDataSourceId: {
                required: "请选择数据源"
            },
            TypeId: {
                required: "请选择类型"
            }
        }
    },
    DataSource: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            Description: {
                maxlength: 500
            }
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            Description: {
                maxlength: "描述长度不能超过500个字符"
            }
        }
    },
    PaymentBank: {
        Rules: {
            Name: CommonValidation.Rules.Name,
            Description: {
                maxlength: 500
            },
            Sort: CommonValidation.Rules.Sort,
            Payee: {
                required: true
            },
            LanguageId: {
                required: true,
            },
            Account: {
                required: true
            }
        },
        Messages: {
            Name: CommonValidation.Messages.Name,
            Sort: CommonValidation.Messages.Sort,
            Description: {
                maxlength: "描述长度不能超过500个字符"
            },
            Payee: {
                required: "请输入收款人"
            },
            LanguageId: {
                required: "请选择语言",
            },
            Account: {
                required: "请输入帐号"
            }
        }
    }
};