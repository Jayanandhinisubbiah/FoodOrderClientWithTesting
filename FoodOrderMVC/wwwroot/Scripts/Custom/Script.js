let btnsend = document.querySelector('button');
let message = document.querySelector('h1');
btnsend.addEventListener('click', () => {
    btnsend.innerText = 'Sending...';
    setTimeout(() => {
        btnsend.style.display = "none";
        message.innerText = 'Message Sent';

    }, 1000);
});
