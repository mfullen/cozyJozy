/**
Feeding model class
*/
function FEED(data) {
    this.id = ko.observable(data.id);
    this.startTime = ko.observable(data.startTime).extend({ required: true });
    this.endTime = ko.observable(data.endTime).extend({ required: true });
    this.deliveryType = ko.observable(data.deliveryType).extend({ required: true });
    this.amount = ko.observable(data.amount).extend({ numeric: 2 });
    this.dateReported = ko.observable(data.dateReported);
    this.childId = ko.observable(data.childId);
    this.spitUp = ko.observable(data.spitUp);
    this.notes = ko.observable(data.notes);
}

/**
Diaper change model class
*/
function DC(data) {
    this.id = ko.observable(data.id);
    this.occurredOn = ko.observable(data.occurredOn).extend({ required: true });
    this.notes = ko.observable(data.notes);
    this.urine = ko.observable(data.urine);
    this.stool = ko.observable(data.stool);
    this.childId = ko.observable(data.childId);
}

/**
Child model class
*/
function ChildClass(data) {
    this.id = ko.observable(data.id);
    this.dateOfBirth = ko.observable(data.dateOfBirth).extend({ required: true });
    this.firstName = ko.observable(data.firstName).extend({ required: true });
    this.middleName = ko.observable(data.middleName);
    this.lastName = ko.observable(data.lastName);
    this.male = ko.observable(data.male);
}

/**
ChildPermission is an aggregrate class that contains a child and the access settings for that user in the app
*/
function ChildPermission(data) {
    this.child = ko.observable(new ChildClass(data.child));
    this.readOnly = data.readOnly;
}

/**
Permission class represents the actual data contained in granting a user permissions in the application
*/
function Permission(data) {
    this.id = ko.observable(data.id);
    this.user = ko.observable(new User(data.user)).extend({ required: true });
    this.child = ko.observable(new ChildClass(data.child));
    this.readOnly = ko.observable(data.readOnly);
    this.childId = ko.observable(data.childId);
    this.title = ko.observable(data.title).extend({ required: true });
}

/**
User model class
*/
function User(data) {
    this.id = ko.observable(data.id);
    this.userName = ko.observable(data.userName);
    this.email = ko.observable(data.email);
    this.profileImageUrl = ko.observable(data.profileImageUrl);
}

/**
Measurement model class
*/
function MeasurementClass(data) {
    this.id = ko.observable(data.id);
    this.dateRecorded = ko.observable(data.dateRecorded).extend({ required: true });
    this.height = ko.observable(data.height);
    this.weight = ko.observable(data.weight);
    this.childId = ko.observable(data.childId);
    this.headCircumference = ko.observable(data.headCircumference);
}

/**
The dashboard statistics model class
*/
function DashboardStats(data) {
    this.dateOfBirth = ko.observable(data.dateOfBirth);
    this.lastFeeding = ko.observable(data.lastFeeding);
    this.lastDiaperChange = ko.observable(data.lastDiaperChange);
    this.totalFeedings = ko.observable(data.totalFeedings);
    this.totalDiaperChanges = ko.observable(data.totalDiaperChanges);
    this.numberOfRecentFeedings = ko.observable(data.numberOfRecentFeedings);
    this.numberOfRecentDiaperChanges = ko.observable(data.numberOfRecentDiaperChanges);
    this.recentAmountPerFeed = ko.observable(data.recentAmountPerFeed);
    this.totalAmountPerFeed = ko.observable(data.totalAmountPerFeed);
    this.totalRecentAmount = ko.observable(data.totalRecentAmount);
    this.childId = ko.observable(data.childId);
}

/**
Sleep Session data model
*/
function SleepSession(data) {
    this.id = ko.observable(data.id);
    this.startTime = ko.observable(data.startTime).extend({ required: true });
    this.endTime = ko.observable(data.endTime).extend({ required: true });
    this.childId = ko.observable(data.childId);
    this.notes = ko.observable(data.notes);
}


function convertMlToOz(ml) {
    return +(Math.round((ml * 0.033814) + "e+2") + "e-2");
}