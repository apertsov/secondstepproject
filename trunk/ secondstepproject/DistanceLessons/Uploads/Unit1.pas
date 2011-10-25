unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, ExtDlgs;
type
  vec   =  array[1..7,1..7] of real;

  vector = record
    v:      vec;
    meaning:string;
  end;

  vectors = record
    vecs: array[1..50] of vector;
    n,m,count:integer;
  end;



type
  TForm1 = class(TForm)
    allTextImg: TImage;
    curLetterImg: TImage;
    Button1: TButton;
    Button2: TButton;
    Button3: TButton;
    Memo1: TMemo;
    Edit1: TEdit;
    OpenPictureDialog1: TOpenPictureDialog;
    OpenDialog1: TOpenDialog;
    Edit2: TEdit;
    Label1: TLabel;
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure Button3Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;
  lBorder,rBorder,tBorder,bBorder:integer;
  vs:vectors;
  pic,let:boolean;
implementation

{$R *.dfm}

procedure TForm1.Button1Click(Sender: TObject);
begin
if OpenPictureDialog1.Execute then
  begin
  allTextImg.Picture.LoadFromFile(OpenPictureDialog1.FileName);
  pic:=true;
  end;
end;



{feature vector v[i,j]=black[i,j]/allblack}

function calcFeatureVec(img:TImage;lBorder,rBorder,tBorder,bBorder,n,m:integer):vec;
var v:vec;
  i,j,k,l,allBlack,stepX,stepY,black:integer;
  startX,stopX:integer;
  startY,stopY:integer;

begin
stepX:=(rborder-lborder) div n;
stepY:=(bborder-tborder) div m;

for i:=1 to n do
  for j:=1 to m do
   v[i,j]:=0;
allBlack:=0;
with img.Picture.Bitmap.Canvas do
  begin
  for i:=lborder to rborder do
    for j:=tborder to bborder do
       if (pixels[i,j]=clBlack) then allBlack:=allBlack+1;


  for i:=1 to n do
    for j:=1 to m do
    begin
    black:=0;
    startX:=lborder+(i-1)*stepX;
    stopX:=startX+stepX;
    startY:=tborder+(j-1)*stepY;
    stopY:=startY+stepX;
    for l:=startX to stopX do
      for k:=startY to stopY do
      
      if (pixels[l,k]=clblack) then black:=black+1;


      v[i,j]:=(100*black/allblack);

    end;
   end;
 result:=v;
end;

 function norma(a,b:vec;n,m:integer):real;
var i,j:integer;
   norm:real;
begin
norm:=0;
for i:=1 to n do
  for j:=1 to m do
    norm:=norm+abs(a[i,j]-b[i,j]);
result:=norm;
end;



function recognizeVec(letters:vectors;v:vec):string;
var i,minIndex:integer;
  minNorm:real;
begin
minNorm:=norma(v,letters.vecs[1].v,letters.n,letters.m);
minIndex:=1;
for i:=1 to letters.count do
  if (norma(v,letters.vecs[i].v,letters.n,letters.m)<minNorm) then
    begin
    minNorm:=norma(v,letters.vecs[i].v,letters.n,letters.m);
    minIndex:=i;
    end;
result:=letters.vecs[minIndex].meaning;
end;



{ Blank are all cols without  black pixels }

function isBlankCol(img:TImage;col:integer):boolean;
var f:boolean;
    i:integer;
begin
f:=true;
for i:=img.ClientHeight downto 0 do

  if (img.Picture.Bitmap.Canvas.Pixels[col,i]=clBlack) then
    begin
    f:=false;
     end;

result:=f;
end;

procedure skipBlank(img:TImage; var lBorder:integer);
begin
 while(isBlankCol(img,lBorder) and (lBorder<=img.ClientWidth)) do lBorder:=lBorder+1;
end;

procedure findRBorder(img:TImage; lBorder:integer; var tBorder, bBorder, rBorder:integer);
var i:integer;
begin
rBorder:=lBorder;
tBorder:=img.ClientHeight;
bBorder:=0;
while(not isBlankCol(img,rBorder)and (rBorder<=img.ClientWidth)) do
      begin
      rBorder:=rBorder+1;
        i:=img.ClientHeight;
        while (not(img.Picture.Bitmap.Canvas.Pixels[rBorder,i]=clBlack) and (i>=0)) do i:=i-1;
        if (bBorder<i) then bBorder:=i;

        i:=0;
        while (not(img.Picture.Bitmap.Canvas.Pixels[rBorder,i]=clBlack) and (i<=img.ClientHeight)) do i:=i+1;
        if (tBorder>i) then tBorder:=i;
       end;
end;



procedure separateLetter(img:TImage;var lBorder,rBorder,tBorder,bBorder:integer);
begin
skipBlank(img,lBorder);
findRBorder(img,lBorder,tBorder,bBorder,rBorder);
end;



procedure TForm1.Button2Click(Sender: TObject);
var v:vec;
    i:integer;
    vc:vec;
    res:string;
    letterCount:integer;
begin
if ((not pic) or (not let)) then exit;
res:='';
lborder:=0;
letterCount:=strtoint(edit2.text);
i:=0;
while (i<letterCount) do
begin
separateLetter(allTextImg,lBorder,rBorder,tBorder,bBorder);
i:=i+1;
 vc:=calcFeatureVec(allTextImg,lBorder,rBorder,tBorder,bBorder,vs.m,vs.n);
res:=res+recognizeVec(vs,vc);
lborder:=rborder+3;
end;
edit1.Text:=res;
end;

procedure TForm1.Button3Click(Sender: TObject);
var i:integer;
    lb,rb,tb,bb:integer;
begin
if (OpenDialog1.Execute) then
begin
  memo1.Lines.LoadFromFile(OpenDialog1.FileName);
  vs.count:=strtoint(memo1.Lines[0]);
  vs.m:=7;
  vs.n:=7;
  for i:=1 to vs.count do
  begin
  lb:=0;
  vs.vecs[i].meaning:=Memo1.Lines[2*(i-1)+1];
  curLetterImg.Picture.Bitmap.LoadFromFile(Memo1.Lines[2*(i-1)+2]);
  separateLetter(curLetterImg,lb,rb,tb,bb);
  vs.vecs[i].v:=calcFeatureVec(curLetterImg,lb,rb,tb,bb,vs.m,vs.n);
  let:=true;
end;
end;
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
pic:=false;
let:=false;
end;

end.
