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
    let el = document.getElementById('postImage');
    el.src = '';
    el.hidden = true;
});

postConnection.on('ShowPostErrors', (errors) => {
    let section = document.getElementById("postErrors");
    let source = document.getElementById('errorsTemplate').innerHTML;
    let template = Handlebars.compile(source);
    let resultHTML = template({ errors });
    section.innerHTML = resultHTML;
    section.hidden = false;
    setTimeout(() => {
        section.hidden = true;
    }, 7500)
});

postConnection.on('ShowPost', (post, user) => {
    let section = document.getElementById('posts');
    let source = document.getElementById('commentTemplate').innerHTML;
    Handlebars.registerPartial('comment', source);
    source = document.getElementById('postTemplate').innerHTML;
    let template = Handlebars.compile(source);

    let obj = {
        post,
        user,
    }
    let resultHTML = template(obj);
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
    if (parent.classList.contains('commentButton') == false && target.classList.contains('commentButton') == false) {
        return;
    }

    while(parent.className.includes('col-md-12') == false){
        parent = parent.parentElement;
    }

    let section = parent.getElementsByClassName('comments')[0];

    if (section.style.display === "none") {
        section.style.display = "block";
    } else {
        section.style.display = "none";
    }
});

postConnection.on('ShowCommentErrors', (errors, postId) => {
    let section = document
        .querySelector(`input.postId[value="${postId}"]`)
        .parentElement
        .parentElement
        .getElementsByClassName('commentErrors')[0];;
    let source = document.getElementById('errorsTemplate').innerHTML;
    let template = Handlebars.compile(source);
    let resultHTML = template({ errors });
    section.innerHTML = resultHTML;
    section.hidden = false;
    setTimeout(() => {
        section.hidden = true;
    }, 7500)
});

postConnection.on('ShowComment', (comment, postId) => {
    let section = document
        .querySelector(`input.postId[value="${postId}"]`)
        .parentElement
        .parentElement
        .getElementsByClassName('usersComments')[0];
    let source = document.getElementById('commentTemplate').innerHTML;
    let template = Handlebars.compile(source);
    let resultHTML = template(comment);
    section.innerHTML = resultHTML + section.innerHTML;
});

document.getElementById('posts').addEventListener('click', e => {
    e.preventDefault();
    let { target } = e;
    let parent = target.parentElement;
    if(parent.classList.contains('sendComment') == false){
        return;
    }

    let contentEl = parent.parentElement.getElementsByTagName('input')[0];

    while(parent.className.includes('col-md-12') == false){
        parent = parent.parentElement;
    }

    let postId = parent.getElementsByClassName('postId')[0].value;

    postConnection.invoke('CreateComment', contentEl.value, postId)
        .catch(function (err) {
            console.log(err.toString());
        });
    contentEl.value = '';
});

//Like and DisLike
document.getElementById('posts').addEventListener('click', e => {
    e.preventDefault();
    let { target } = e;
    let parent = target.parentElement;
    if(parent.classList.contains('likeButton') == false && target.classList.contains('likeButton') == false){
        return;
    }

    while(parent.className.includes('col-md-12') == false){
        parent = parent.parentElement;
    }

    let postId = parent.getElementsByClassName('postId')[0].value;
    let pEl = target.parentElement.getElementsByTagName('p')[0];
    let svgEl = target.parentElement.getElementsByTagName('svg')[0];
    let imgEl = target.parentElement.getElementsByTagName('img')[0];

    if (pEl.textContent == 'Like') {
        postConnection.invoke('LikePost', postId);
        pEl.textContent = 'Dislike';
        svgEl.style.display = 'none';
        imgEl.hidden = false;
    } else {
        postConnection.invoke('DisLikePost', postId);
        pEl.textContent = 'Like';
        svgEl.style.display = 'block';
        imgEl.hidden = true;
    }
});

postConnection.on('ChangeLikesCount', (postId, count) => {

});
