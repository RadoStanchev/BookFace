let friendshipConnection = new signalR.HubConnectionBuilder().withUrl('/friendshipHub').build();
friendshipConnection.start()
    .then(() => console.log('Friendships connected'))
    .catch(function (err) {
        console.log(err.toString());
    });

document.getElementById('buttons').addEventListener('click', e => {
    e.preventDefault();
    let { target } = e;
    if (target.nodeName.toLowerCase() != "button") {
        return;
    }

    let parent = target.parentElement;;
    let friendId = parent.getElementsByTagName('input')[0].value;
    switch (target.value) {
        case 'AddFriend':
            friendshipConnection.invoke('SendRequest', friendId)
                .then(() => {
                    parent.removeChild(target);
                    let source = document.getElementById('waitingTemplate').innerHTML;
                    let template = Handlebars.compile(source);
                    let resultHTML = template();
                    parent.innerHTML = resultHTML + parent.innerHTML;
                });
            break;
        case 'Accept':
            friendshipConnection.invoke('SendAccept', friendId)
                .then(() => {
                    removeAcceptAndDeny(parent);
                    let source = document.getElementById('breakUpTemplate').innerHTML;
                    let template = Handlebars.compile(source);
                    let resultHTML = template();
                    parent.innerHTML = resultHTML + parent.innerHTML;
                });
            break;
        case 'Deny':
            friendshipConnection.invoke('SendDeny', friendId)
                .then(() => {
                    removeAcceptAndDeny(parent);
                    let source = document.getElementById('addFriendTemplate').innerHTML;
                    let template = Handlebars.compile(source);
                    let resultHTML = template();
                    parent.innerHTML = resultHTML + parent.innerHTMLHTML;
                });
            break;
        case 'BreakUp':
            friendshipConnection.invoke('SendBreakUp', friendId)
                .then(() => {
                    parent.removeChild(target);
                    let source = document.getElementById('addFriendTemplate').innerHTML;
                    let template = Handlebars.compile(source);
                    let resultHTML = template();
                    parent.innerHTML = resultHTML + parent.innerHTML;
                });
            break;
        case 'Block':
            friendshipConnection.invoke('SendBlock', friendId)
                .then(() => {
                    parent.removeChild(target);
                    let source = document.getElementById('unBlockTemplate').innerHTML;
                    let template = Handlebars.compile(source);
                    let resultHTML = template();
                    parent.innerHTML += resultHTML;
                });
            break;
        case 'UnBlock':
            friendshipConnection.invoke('SendUnBlock', friendId)
                .then(() => {
                    parent.removeChild(target);
                    let source = document.getElementById('blockTemplate').innerHTML;
                    let template = Handlebars.compile(source);
                    let resultHTML = template();
                    parent.innerHTML += resultHTML;
                });
            break;
    }
});

function removeAcceptAndDeny(parent) {
   let acceptButton = parent.querySelectorAll("button[value=Accept]")[0];
   let denyButton = parent.querySelectorAll("button[value=Deny]")[0];
    parent.removeChild(acceptButton);
    parent.removeChild(denyButton);
}