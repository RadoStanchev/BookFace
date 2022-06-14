let chatConnection = new signalR.HubConnectionBuilder().withUrl('/ChatHub').build();
chatConnection.start()
    .then(() => console.log('Chats connected'))
    .catch(function (err) {
        console.log(err.toString());
    });

chatConnection.on('GetNewOnline', (user) => {
    let section = document.getElementById('friends');
    let source = document.getElementById('friendTemplate').innerHTML;
    let template = Handlebars.compile(source);
    let resultHTML = template(user);
    section.innerHTML += resultHTML;
});

chatConnection.on('GetAllOnlineFriends', (users) => {
    let partialSource = document.getElementById('friendTemplate').innerHTML;
    Handlebars.registerPartial('user', partialSource);
    let section = document.getElementById('friends');
    let source = document.getElementById('allFriendsTemplate').innerHTML;
    let template = Handlebars.compile(source);
    let resultHTML = template({ users });
    section.innerHTML = resultHTML;
});

chatConnection.on('RemoveFriend', (userId) => {
    let section = document.getElementById('friends');
    let child = section.querySelectorAll(`input[value="${userId}"]`)[0].parentElement;
    section.removeChild(child);
});

chatConnection.on('ShowChat', (chat, isSolo) =>{
    let topSection = document.getElementsByClassName('chat-header')[0];
    let source = document.getElementById('topChatTemplate').innerHTML;
    let topChatTemplate = Handlebars.compile(source);
    let resultHTML = topChatTemplate(chat);
    topSection.innerHTML = resultHTML;

    let bottomSection = document.getElementsByClassName('chat-history')[0].getElementsByTagName('ul')[0];
    bottomSection.innerHTML = '';
    source = document.getElementById('myMessageTemplate').innerHTML;
    let myMessageTemplate = Handlebars.compile(source);
    source = document.getElementById('othersMessageTemplate').innerHTML;
    let othersMessageTemplate = Handlebars.compile(source);
    let friendId = document.getElementById('friendId').value;
    
    chat.messages.forEach(message => {
        if(message.creatorId != friendId || isSolo){
            resultHTML = myMessageTemplate(message);
        } else {
            resultHTML = othersMessageTemplate(message);
        }
        bottomSection.innerHTML += resultHTML;
    });
});

document.getElementById('friends').addEventListener('click', (e) => {
    e.preventDefault();
    let { target } = e;

    if(target.nodeName.toLowerCase() == 'ul'){
        return;
    }

    let parent = target;
    while(parent.nodeName.toLowerCase() != 'li'){
        parent = parent.parentElement;
    }

    let friendId = parent.getElementsByTagName('input')[0].value;

    let chatInput = document.getElementById('chatId');
    if(chatInput !== null){
        chatConnection.invoke('LeaveGroup', chatInput.value)
        .then(() => console.log('Leave chat'))
        .catch(function (err) {
            console.log(err.toString());
        });
    }

    chatConnection.invoke('JoinGroup', friendId)
        .then(() => console.log('Join chat'))
        .catch(function (err) {
            console.log(err.toString());
        });
});

document.getElementsByClassName('chat-message')[0].addEventListener('click', (e) => {
    let { target } = e;
    if(target.nodeName.toLowerCase() !== 'span' && target.nodeName.toLowerCase() !== 'i'){
        return
    }
    sendMessage(e)
});
document.getElementsByClassName('chat-message')[0].addEventListener('keypress', (e) => {
    if (e.key !== "Enter") {
        return;
    }
    sendMessage(e)
});

function sendMessage(e) {
    let chatId = document.getElementById('chatId').value;
    let inputEl = document.getElementById('messageContent'); 
    let content = inputEl.value;

    if(!content || content === '' ){
        return;
    }

    inputEl.value = '';
    chatConnection.invoke('SendMessage', content, chatId);
};

chatConnection.on('RecieveMessage', (message, isSolo) => {
    let bottomSection = document.getElementsByClassName('chat-history')[0].getElementsByTagName('ul')[0];
    let source = document.getElementById('myMessageTemplate').innerHTML;
    let myMessageTemplate = Handlebars.compile(source);
    source = document.getElementById('othersMessageTemplate').innerHTML;
    let othersMessageTemplate = Handlebars.compile(source);
    let friendId = document.getElementById('friendId').value;
    
    if(message.creatorId != friendId || isSolo){
        resultHTML = myMessageTemplate(message);
    } else {
        resultHTML = othersMessageTemplate(message);
    }
    bottomSection.innerHTML += resultHTML;
});

chatConnection.on('CloseChat', () => {

});

document.getElementById('searchBar').addEventListener('click', (e) => search(e));
document.getElementById('searchBar').addEventListener('change', (e) => search(e));

function search(e){
    e.preventDefault();
    let searchTerms = document.getElementById('searchBar').getElementsByTagName('input')[0].value;
    chatConnection.invoke('Search', searchTerms);
}