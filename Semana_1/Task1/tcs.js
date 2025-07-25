a=process.argv.slice(2);
if(a.length===0)return'';
let l='',s=a.reduce((x,y)=>x.length<=y.length?x:y);for(i=0;i<s.length;i++)for(j=i+1;j<=s.length;j++){t=s.slice(i,j);a.every(x=>x.includes(t))&&t.length>l.length&&(l=t)}console.log(l)