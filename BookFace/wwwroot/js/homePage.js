//Show Image
document.getElementById('postImageUrl').addEventListener('change', e => {
    e.preventDefault();
    let el = document.getElementById('postImage');
    el.src = e.target.value;
    el.hidden = false;
});

//Creating post
let postConnection = new signalR.HubConnectionBuilder().withUrl('/postHub').build();
postConnection.start()
    .then(() => console.log('Posts connected'))
    .catch(function (err) {
        console.log(err.toString());
    });

document.getElementById('sendPost').addEventListener('click', e => {
    e.preventDefault();
    let contentEl = document.getElementById('postContent');
    let imageUrlEl = document.getElementById('postImageUrl');
    postConnection.invoke('CreatePost', contentEl.value, imageUrlEl.value)
        .catch(function (err) {
            console.log(err.toString());
        });
    contentEl.value = "";
    imageUrlEl.value = "";
});

postConnection.on('ShowPostErrors', (errors) => {
    let section = document.getElementById("postErrors");
    let source = document.getElementById('postErrorsTemplate').innerHTML;
    let template = Handlebars.compile(source);
    let resultHTML = template({ errors });
    section.innerHTML = resultHTML;
    section.hidden = false;
    setTimeout(() => {
        section.hidden = true;
    }, 7500)
});

postConnection.on('ShowPost', (post) => {
    let section = document.getElementById('posts');
    let source = document.getElementById('postTemplate').innerHTML;
    let template = Handlebars.compile(source);
    let resultHTML = template({ post });
    section.innerHTML = resultHTML + section.innerHTML;
});

//Send Request
let friendshipConnection = new signalR.HubConnectionBuilder().withUrl('/friendshipHub').build();
friendshipConnection.start()
    .then(() => console.log('Friendships connected'))
    .catch(function (err) {
        console.log(err.toString());
    });

document.getElementById('suggestions').addEventListener('click', e => {
    e.preventDefault();
    let { target } = e;
    if (target.nodeName.toLowerCase() != "button" && target.nodeName.toLowerCase() != "svg") {
        return;
    }
    let parent = target.parentElement;;

    if (target.nodeName == "svg") {
        parent = parent.parentElement;
    }

    let friendId = parent.getElementsByTagName('input')[0].value;
    friendshipConnection.invoke('SendRequest', friendId)
        .then(() => {
            console.log("Succesful request");
            let source = document.getElementById('succesfulRequest').innerHTML;
            let template = Handlebars.compile(source);
            let resultHTML = template();
            let button = parent.getElementsByTagName('button')[0]
            parent.removeChild(button);
            parent.innerHTML += resultHTML;
        })
        .catch(function (err) {
            console.log(err.toString());
        });
});

//Creating Comment
document.getElementById('posts').addEventListener('click', e => {
    let { target } = e;
    let parent = target.parentElement;
    if (parent.classList.contains('commentButton') == false) {
        return;
    }

    if (parent.style.display === "none") {
        parent.style.display = "block";
    } else {
        parent.style.display = "none";
    }
})