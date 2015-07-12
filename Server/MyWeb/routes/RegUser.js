

var mongoose = require('mongoose');
var User = require('../models/User.js');

exports.regNewUser = function (req, res)
{
    var newUser = new User({
        name: req.body.username,
        password: req.body.password,
    });

    //�z���Ñ����Ƿ��ѽ�����
    User.get(newUser.name, function(err, user) {
        if (user)
            err = 'Username already exists.';
        if (err) {
            req.flash('error', err);
            return res.redirect('/reg');
        }
        //��������ڄt�����Ñ�
        newUser.save(function(err) {
            if (err) {
                req.flash('error', err);
                return res.redirect('/reg');
            }
            req.session.user = newUser;
            req.flash('success', '�]�Գɹ�');
            res.redirect('/');
        });
    });
}


