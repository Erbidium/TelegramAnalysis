from sqlalchemy import ForeignKey, func, Integer
from sqlalchemy.orm import Mapped
from sqlalchemy.orm import mapped_column
from models.base import Base
import datetime
from sqlalchemy import String, DateTime

class Post(Base):
     __tablename__ = "posts"

     id: Mapped[int] = mapped_column(primary_key=True)
     channelid: Mapped[int] = mapped_column(Integer())
     text: Mapped[str] = mapped_column(String(30))
     hash: Mapped[int] = mapped_column()
     viewscount: Mapped[int] = mapped_column()
     createdat: Mapped[datetime.datetime] = mapped_column(DateTime(timezone=True), server_default=func.now())
     editedat: Mapped[datetime.datetime] = mapped_column(DateTime(timezone=True), server_default=func.now())
     parsedat: Mapped[datetime.datetime] = mapped_column(DateTime(timezone=True), server_default=func.now())


     def repr(self) -> str:
         return f"Post(id={self.id!r}, text={self.text!r}, hash={self.hash!r}," \
                f"viewscount={self.viewscount!r}, createdat={self.createdat!r}, editedat={self.editedat!r}," \
                f"parsedat={self.parsedat!r})"


class Postreactions(Base):
     __tablename__ = "postreactions"

     id: Mapped[int] = mapped_column(primary_key=True)
     postid: Mapped[int] = mapped_column(ForeignKey("post.id"))


     def __repr__(self) -> str:
         return f"Postreaction(id={self.id!r}, postid={self.postid!r})"
