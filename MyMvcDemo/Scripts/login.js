$(function () {
    var cookie_login_user = "shopfun.v2.cookie.login.user";
    var cookieUser = $.cookie(cookie_login_user);
    if (cookieUser && cookieUser.length && cookieUser != "null") {
        $("#RememberMe").attr("checked", "checked");
        $("#UserName").val(cookieUser).keyup();
    };
    $("#aCaptcha").click(function () {
        var $captchaImg = $("#imgCaptcha");
        var src = $captchaImg.attr('src') + '?';
        $captchaImg.attr('src', src);
        $("#Captcha").val("");

        setTimeout(function () {
            $("#Captcha").focus();
        }, 500);

        return false;
    });
    function errorPlacement(error, element) {
        var errText = $(error).html();
        if (error[0].htmlFor == "Captcha" && $("input[name='" + error[0].htmlFor + "']").val().length>0) {
            $(".captchalogin .error").html("验证码不正确");
        } else {
            $("input[name='" + error[0].htmlFor + "']").attr("placeholder", errText);
        }
    }
    function successPlacement(element, errorClass) {
        $(element).closest(".control-group").removeClass("error");
    }
    $("#formLogin").validate(
        {
            errorPlacement: errorPlacement,
            unhighlight: successPlacement,
            rules: {
                UserName: {
                    required: true
                },
                Password: {
                    required: true
                },
                Captcha: {
                    required: true,
                    remote: {
                        url: lc,
                        type: "post",
                        data: {
                            captcha: function () {
                                return $("#Captcha").val();
                            }
                        }
                    }
                }
            },
            messages:
                {
                    UserName:
                        {
                            required: "请输入用户名或邮箱"
                        },
                    Password:
                        {
                            required: "请输入登录密码"
                        },
                    Captcha:
                        {
                            required: "请输入验证码",
                            remote: "验证码不正确"
                        }
                },
            submitHandler: function (form) {
                $(form).ajaxSubmit(
                {
                    dataType: "json",
                    beforeSubmit: function (formData, $form, options) {
                        util.block("#login-box .widget-body");
                    },
                    success: function (responseText, statusText, xhr, element) {
                        util.unblock("#login-box .widget-body");
                        if (responseText.success) {
                            if ($("#RememberMe").is(":checked")) {
                                $.cookie(cookie_login_user, $("#UserName").val());
                            }
                            else {
                                $.cookie(cookie_login_user,null);
                            }
                            window.location = responseText.returnUrl;
                        } else {
                            util.err("登录失败", responseText.errors);
                        }
                        $("#aCaptcha").click();
                    }
                });
            }
        });
});