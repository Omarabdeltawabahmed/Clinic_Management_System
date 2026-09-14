const toggle = document.querySelector('.nav-toggle');
const nav = document.querySelector('.main-nav');
const auth = document.querySelector('.auth-actions');
if (toggle) toggle.addEventListener('click', () => { nav?.classList.toggle('open'); auth?.classList.toggle('open'); });

const dots = [...document.querySelectorAll('.hero-dots button')];
let current = 0;
function setDot(i){ current=(i+dots.length)%dots.length; dots.forEach((d,idx)=>d.classList.toggle('active',idx===current)); }
dots.forEach((d,i)=>d.addEventListener('click',()=>setDot(i)));
document.querySelector('.hero-arrow.left')?.addEventListener('click',()=>setDot(current-1));
document.querySelector('.hero-arrow.right')?.addEventListener('click',()=>setDot(current+1));
setInterval(()=>{ if(dots.length) setDot(current+1); }, 5000);
