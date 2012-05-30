'''
Created on May 25, 2012

@author: Eyal
'''

import tkinter
from PIL import Image, ImageTk
import threading

if __name__ == '__main__':
    root = tkinter.Tk()
    root.geometry("600x400")
    root.title("Note Taker")

    fileframe = tkinter.Frame(root, background="red")
    
    jsonframe = tkinter.Frame(fileframe, background="green")
    jsonText = tkinter.Text(jsonframe, height=10)
    jsonText.pack(fill=tkinter.BOTH)
    jsonframe.pack(fill=tkinter.BOTH, side=tkinter.BOTTOM)

    
    imageFile = r'c:\users\eyal\Pictures\Thailand 2011\Bangkok\DSCF9015.JPG'
    img1 = Image.open(imageFile)
    img1.thumbnail((500,500))
    pImg1 = ImageTk.PhotoImage(img1)
    imageLabel = tkinter.Label(fileframe, image=pImg1)
    imageLabel.image = img1
    imageLabel.pImage = pImg1
    imageLabel.pack(fill=tkinter.BOTH, expand=1)
    fileframe.pack(fill=tkinter.BOTH, side=tkinter.LEFT, expand=1)
    
    tagsframe = tkinter.Frame(root, background="blue")
    newTag = tkinter.Text(tagsframe, height=1)
    newTag.pack(side=tkinter.TOP, fill=tkinter.X)
    b1 = tkinter.Button(tagsframe, text="SampleTag")
    b1.pack()
    b2 = tkinter.Button(tagsframe, text="SampleTag")
    b2.pack()
    tagsframe.pack(fill=tkinter.Y, side=tkinter.LEFT, expand=1)
    root.mainloop() 