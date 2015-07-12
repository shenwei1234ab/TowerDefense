

var mongoose = require('mongoose');
var User = require('../models/User.js');

exports.regNewUser = function (req, res)
{
    var newUser = new User({
        name: req.body.username,
        password: req.body.password,
    });

    //檢查用戶名是否已經存在
    User.get(newUser.name, function(err, user) {
        if (user)
            err = 'Username already exists.';
        if (err) {
            req.flash('error', err);
            return res.redirect('/reg');
        }
        //如果不存在則新增用戶
        newUser.save(function(err) {
            if (err) {
                req.flash('error', err);
                return res.redirect('/reg');
            }
            req.session.user = newUser;
            req.flash('success', '註冊成功');
            res.redirect('/');
        });
    });
}


