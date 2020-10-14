﻿class User {

    constructor(id, username, firstname, lastname, nickname, fullname, email, isactive, isadmin, issuperuser, datecreated, dateupdated) {

        if (!arguments.length) {

            this.id = 0;
            this.userName = "";
            this.firstName = "";
            this.lastName = "";
            this.nickName = "";
            this.fullName = "";
            this.email = "";
            this.isActive = false;
            this.isAdmin = false;
            this.isSuperUser = "";
            this.dateCreated = new Date('0001-01-01T00:00:00Z');
            this.dateUpdated = new Date('0001-01-01T00:00:00Z');
            this.isLoggedIn = false;

        } else {

            this.id = id;
            this.userName = username;
            this.firstName = firstname;
            this.lastName = lastname;
            this.nickName = nickname;
            this.fullName = fullname;
            this.email = email;
            this.isActive = isactive;
            this.isActive = isadmin;
            this.isSuperUser = issuperuser;
            this.dateCreated = new Date(datecreated);
            this.dateUpdated = new Date(dateupdated);
            this.isLoggedIn = false;
        }
    }

    login() {

        this.isLoggedIn = true;
    }

    logout() {

        this.id = 0;
        this.userName = "";
        this.firstName = "";
        this.lastName = "";
        this.nickName = "";
        this.fullName = "";
        this.email = "";
        this.isActive = false;
        this.isAdmin = false;
        this.isSuperUser = "";
        this.dateCreated = new Date('0001-01-01T00:00:00Z');
        this.dateUpdated = new Date('0001-01-01T00:00:00Z');
        this.isLoggedIn = false;
    }
}

export default User;